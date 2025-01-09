using MassTransit;
using Microsoft.Extensions.Logging;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities
{
    public class PaymentFailedEventActivity: IStateMachineActivity<OrderSagaState, PaymentFailedEvent>
    {
        private readonly ITopicProducer<OrderFailedEvent> _orderFailedEventProducer;
        private readonly ITopicProducer<StockRollBackMessage> _stockRollbackMessageProducer;
        private readonly ILogger<PaymentFailedEventActivity> _logger;

        public PaymentFailedEventActivity(ITopicProducer<OrderFailedEvent> orderFailedEventProducer, 
            ITopicProducer<StockRollBackMessage> stockRollbackMessageProducer,
            ILogger<PaymentFailedEventActivity> logger) 
        {
            _orderFailedEventProducer = orderFailedEventProducer;
            _stockRollbackMessageProducer = stockRollbackMessageProducer;
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, PaymentFailedEvent> context, IBehavior<OrderSagaState, PaymentFailedEvent> next)
        {
            await _orderFailedEventProducer.Produce(
               new OrderFailedEvent()
               {
                   CorrelationId = context.Message.CorrelationId,
                   CustomerId = context.Saga.CustomerId,
                   OrderId = context.Saga.OrderId.GetValueOrDefault(),
                   ErrorMessage = context.Message.ErrorMessage
               },
               context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("OrderFailedEvent published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);

            await _stockRollbackMessageProducer.Produce(
               new StockRollBackMessage()
               {
                   CorrelationId = context.Message.CorrelationId,
                   OrderItemList = context.Message.OrderItemList,
               },
               context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("StockRollBackMessage published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, PaymentFailedEvent, TException> context, IBehavior<OrderSagaState, PaymentFailedEvent> next) where TException : Exception
        {
            // always call the next activity in the behavior
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("payment-failed-closed");
        }
    }
}
