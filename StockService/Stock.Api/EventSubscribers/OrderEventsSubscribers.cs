using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using SharedUtils;
using SharedUtils.IntegrationEvents;
using Stock.Domain.Entities;
using Stock.Infrastructure.Repository;

namespace Stock.Api.EventSubscribers
{
    public class OrderEventsSubscribers : IOrderEventsSubscribers, ICapSubscribe
    {
        private readonly IRepository<StockEntity> _stockRepository;
        private readonly ILogger<OrderEventsSubscribers> _logger;
        private readonly ICapPublisher _capPublisher;

        public OrderEventsSubscribers(IRepository<StockEntity> stockRepository, ICapPublisher capPublisher, ILogger<OrderEventsSubscribers> logger)
        {
            _stockRepository = stockRepository;
            _capPublisher = capPublisher;
            _logger = logger;
        }

        [CapSubscribe(TopicNames.OrderCreated)]
        public async Task HandleOrderCreated(OrderCreatedEvent orderCreatedEvent)
        {
            try
            {
                var productIds = orderCreatedEvent.OrderItemList.Select(x => x.ProductId).ToList();
                var stockItems = (await _stockRepository.GetAsync(x => productIds.Contains(x.ProductId))).ToDictionary(x => x.ProductId);
                var stockReserved = false;

                foreach (var orderedItem in orderCreatedEvent.OrderItemList)
                {
                    if (!stockItems.TryGetValue(orderedItem.ProductId, out var stock) || stock.Count < orderedItem.Count)
                    {
                        _logger.LogInformation("Stock issue for ProductId: {ProductId} and CorrelationId: {CorrelationId}",
                            orderedItem.ProductId, orderCreatedEvent.CorrelationId);

                        await _capPublisher.PublishAsync(TopicNames.StockReservationFailed, new StockReservationFailedEvent
                        {
                            CorrelationId = orderCreatedEvent.CorrelationId,
                            ErrorMessage = $"Insufficient stock for ProductId: {orderedItem.ProductId}"
                        });
                        return;
                    }

                    stock.Count -= orderedItem.Count;
                    _stockRepository.Update(stock);
                    stockReserved = true;
                }

                if (stockReserved)
                {
                    await _stockRepository.SaveChangesAsync();

                    _logger.LogInformation("Stock reserved for CorrelationId: {CorrelationId}", orderCreatedEvent.CorrelationId);
                    _capPublisher.Publish(TopicNames.StockReserved, new StockReservedEvent
                    {
                        CorrelationId = orderCreatedEvent.CorrelationId,
                        OrderItemList = orderCreatedEvent.OrderItemList
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing OrderCreatedEvent for CorrelationId: {CorrelationId}", orderCreatedEvent.CorrelationId);
                throw;
            }
        }
    }
}
