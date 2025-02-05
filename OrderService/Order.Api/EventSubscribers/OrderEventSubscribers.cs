﻿using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Order.Application;
using Order.Application.Dtos;
using Order.Domain.Entities;
using SharedUtils;
using SharedUtils.IntegrationEvents;

namespace Order.Api.EventSubscribers
{
    public class OrderEventSubscribers:IOrderEventSubscribers, ICapSubscribe
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderEventSubscribers> _logger;

        public OrderEventSubscribers(IOrderService orderService, ILogger<OrderEventSubscribers> logger) 
        {
            _orderService = orderService;
            _logger = logger;
        }

        [CapSubscribe(TopicNames.OrderCompleted)]
        public async Task HandleOrderCompleted(OrderCompletedEvent orderCompletedEvent)
        {
            if(string.IsNullOrEmpty(orderCompletedEvent.CustomerId)) 
                return;
            await _orderService.UpdateOrder(new UpdateOrderDto()
            {
                Id = orderCompletedEvent.OrderId,
                Status = OrderStatusDto.Complete
            });
            _logger.LogInformation("Order with Id: {MessageOrderId} completed successfully", orderCompletedEvent.OrderId);
        }

        [CapSubscribe(TopicNames.OrderFailed)]
        public async Task HandleOrderFailed(OrderFailedEvent orderFailedEvent)
        {
            if (string.IsNullOrEmpty(orderFailedEvent.CustomerId))
                return;
            await _orderService.UpdateOrder(new UpdateOrderDto()
            {
                Id = orderFailedEvent.OrderId,
                Status = OrderStatusDto.Fail,
                ErrorMessage = orderFailedEvent.ErrorMessage
            });
            _logger.LogInformation("Order with Id: {MessageOrderId} completed successfully", orderFailedEvent.OrderId);
        }
    }
}
