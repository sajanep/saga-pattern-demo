using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Dtos;
using SharedUtils;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ICapPublisher _capPublisher;

        public TestController(ILogger<TestController> logger,
            ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
            _logger = logger;
        }

        /// <summary>
        /// Publishes test messages and events to create Kafka topics via Cap publish
        /// </summary>
        /// <returns></returns>
        [HttpPost("publish-dummy-messages")]
        public async Task<IActionResult> PublishDummyMessages()
        {
            var correlationId = Guid.NewGuid();
            // #1
            var createOrderMessage = new CreateOrderMessage()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.CreateOrder, createOrderMessage);

            // #2
            var orderCreatedEvent = new OrderCreatedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.OrderCreated, orderCreatedEvent);

            // #3
            var stockReservedEvent = new StockReservedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.StockReserved, stockReservedEvent);

            // #4
            var stockReservationFailedEvent = new StockReservationFailedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.StockReservationFailed, stockReservationFailedEvent);

            // #5
            var completePaymentMessage = new CompletePaymentMessage()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.CompletePayment, completePaymentMessage);

            // #6
            var paymentCompletedEvent = new PaymentCompletedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.PaymentCompleted, paymentCompletedEvent);

            // #7
            var paymentFailedEvent = new PaymentFailedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.PaymentFailed, paymentFailedEvent);

            // #8
            var orderFailedEvent = new OrderFailedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.OrderFailed, orderFailedEvent);

            // #9
            var orderCompletedEvent = new OrderCompletedEvent()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.OrderCompleted, orderCompletedEvent);

            // #10
            var stockRollBackMessage = new StockRollBackMessage()
            {
                CorrelationId = correlationId
            };
            _capPublisher.Publish(TopicNames.StockRollback, stockRollBackMessage);

            return Ok();
        }
    }
}
