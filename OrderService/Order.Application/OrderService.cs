using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Application.Dtos;
using Order.Application.Mappers;
using Order.Domain.Entities;
using Order.Infrastructure.Repository;
using SharedUtils;
using SharedUtils.Messages;


namespace Order.Application
{
    public class OrderService:IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly ICapPublisher _capPublisher;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<OrderEntity> orderRepository, ICapPublisher capPublisher) 
        {
            _orderRepository = orderRepository;
            _capPublisher = capPublisher;
        }
       
        public async Task<int> CreateOrder(CreateOrderDto createOrderDto)
        {
            var newOrder = new OrderEntity
            {
                CustomerId = createOrderDto.CustomerId,
                Status = OrderStatus.Pending,
                PaymentAccountId = createOrderDto.PaymentAccountId,
                OrderItemList = createOrderDto.OrderItemList.Select(item => new OrderItem
                {
                    Price = item.Price,
                    ProductId = item.ProductId,
                    Count = item.Count,
                }).ToList()
            };

            await _orderRepository.AddAsync(newOrder);
            await _orderRepository.SaveChangesAsync();

            var createOrderMessage = new CreateOrderMessage()
            {
                CorrelationId = Guid.NewGuid(),
                CustomerId = newOrder.CustomerId,
                OrderId = newOrder.Id,
                PaymentAccountId = newOrder.PaymentAccountId,
                TotalPrice = newOrder.OrderItemList.Sum(x => x.Price * x.Count),
                OrderItemList = newOrder.OrderItemList.Select(item => new SharedUtils.Dtos.OrderItem
                {
                    Count = item.Count,
                    ProductId = item.ProductId
                }).ToList()
            };

            _capPublisher.Publish(TopicNames.CreateOrder, createOrderMessage);

            return newOrder.Id;
        }

        public async Task<GetOrderDto> GetOrder(int id)
        {
            var orderDto = new GetOrderDto();
            var orderEntity = await _orderRepository.GetByIdAsync(id, x => x.OrderItemList);
            if (orderEntity != null)
            {
                orderDto.Id = orderEntity.Id;
                orderDto.CustomerId = orderEntity.CustomerId;
                orderDto.PaymentAccountId = orderEntity.PaymentAccountId;
                orderDto.ErrorMessage = orderEntity.ErrorMessage;
                orderDto.Status = OrderStatusMapper.ToDto(orderEntity.Status);
                orderDto.OrderItemList = orderEntity.OrderItemList.Select(item => new OrderItemDto
                {
                    Price = item.Price,
                    ProductId= item.ProductId,
                    Count = item.Count,

                }).ToList();
            }
            return orderDto;
        }

        public async Task UpdateOrderStatus(OrderStatusDto orderStatusDto, string customerId)
        {
            var orderEntity = (await _orderRepository.GetAsync(x => x.CustomerId == customerId)).FirstOrDefault();
            if (orderEntity != null)
            {
                orderEntity.Status = OrderStatusMapper.ToDomain(orderStatusDto);
                _orderRepository.Update(orderEntity);
                await _orderRepository.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("Order not found for the customer id {customerId}", customerId);
            }
        }
    }
}
