using MassTransit;
using Microsoft.Extensions.Logging;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities
{
    public class StockReservedEventActivity : IStateMachineActivity<OrderSagaState, StockReservedEvent>
    {
        private readonly ITopicProducer<string, CompletePaymentMessage> _producer;
        private readonly ILogger<StockReservedEventActivity> _logger;

        public StockReservedEventActivity(ITopicProducer<string, CompletePaymentMessage> producer, ILogger<StockReservedEventActivity> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, StockReservedEvent> context, IBehavior<OrderSagaState, StockReservedEvent> next)
        {
            await _producer.Produce(context.Message.CorrelationId.ToString(),
              new CompletePaymentMessage()
              {
                  CorrelationId = context.Message.CorrelationId,
                  OrderItemList = context.Message.OrderItemList,
                  CustomerId = context.Saga.CustomerId,
                  PaymentAccountId = context.Saga.PaymentAccountId,
                  TotalPrice = context.Saga.TotalPrice.GetValueOrDefault(),
              },
              context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("CompletePaymentMessage published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, StockReservedEvent, TException> context, IBehavior<OrderSagaState, StockReservedEvent> next) where TException : Exception
        {
            // always call the next activity in the behavior
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("stock-reserved-closed");
        }
    }
}
