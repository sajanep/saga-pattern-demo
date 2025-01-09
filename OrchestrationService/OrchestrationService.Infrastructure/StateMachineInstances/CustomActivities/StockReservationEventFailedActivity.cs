using MassTransit;
using Microsoft.Extensions.Logging;
using SharedUtils.IntegrationEvents;

namespace OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities
{
    public class StockReservationEventFailedActivity : IStateMachineActivity<OrderSagaState, StockReservationFailedEvent>
    {
        private readonly ITopicProducer<string, OrderFailedEvent> _producer;
        private readonly ILogger<StockReservationEventFailedActivity> _logger;

        public StockReservationEventFailedActivity(ITopicProducer<string, OrderFailedEvent> producer,
            ILogger<StockReservationEventFailedActivity> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, StockReservationFailedEvent> context, IBehavior<OrderSagaState, StockReservationFailedEvent> next)
        {
            await _producer.Produce(context.Message.CorrelationId.ToString(),
             new OrderFailedEvent()
             {
                 CorrelationId = context.Message.CorrelationId,
                 CustomerId = context.Saga.CustomerId,
                 ErrorMessage = context.Message.ErrorMessage,
             },
             context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("OrderFailedEvent published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, StockReservationFailedEvent, TException> context, IBehavior<OrderSagaState, StockReservationFailedEvent> next) where TException : Exception
        {
            // always call the next activity in the behavior
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("stock-reservationfailed-closed");
        }
    }
}
