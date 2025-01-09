using DotNetCore.CAP;
using SharedUtils.IntegrationEvents;
using SharedUtils;
using Stock.Api.EventSubscribers;
using Stock.Domain.Entities;
using Stock.Infrastructure.Repository;
using SharedUtils.Messages;
using Microsoft.EntityFrameworkCore;

namespace Stock.Api.MessageSubscribers
{
    public class StockMessageSubscribers : IStockMessageSubscribers, ICapSubscribe
    {
        private readonly IRepository<StockEntity> _stockRepository;
        private readonly ILogger<OrderEventsSubscribers> _logger;
        private readonly ICapPublisher _capPublisher;

        public StockMessageSubscribers(IRepository<StockEntity> stockRepository,
            ILogger<OrderEventsSubscribers> logger,
            ICapPublisher capPublisher)
        {
            _stockRepository = stockRepository;
            _logger = logger;
            _capPublisher = capPublisher;
        }

        [CapSubscribe(TopicNames.StockRollback)]
        public async Task HandleStockRollback(StockRollBackMessage stockRollbackMessage)
        {
            foreach (var item in stockRollbackMessage.OrderItemList)
            {
                var stock = (await _stockRepository.GetAsync(x => x.ProductId == item.ProductId)).FirstOrDefault();
                if (stock != null)
                {
                    stock.Count += item.Count;
                    _stockRepository.Update(stock);
                    await _stockRepository.SaveChangesAsync();
                }
            }
            _logger.LogInformation($"Stock was released");
        }
    }
}
