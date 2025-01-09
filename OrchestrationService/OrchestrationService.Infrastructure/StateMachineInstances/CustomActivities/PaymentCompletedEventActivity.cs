using MassTransit;
using Microsoft.Extensions.Logging;
using SharedUtils.IntegrationEvents;

namespace OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities
{
    public class PaymentCompletedEventActivity : IStateMachineActivity<OrderSagaState, PaymentCompletedEvent>
    {
        private readonly ITopicProducer<string, OrderCompletedEvent> _producer;
        private readonly ILogger<PaymentCompletedEventActivity> _logger;

        public PaymentCompletedEventActivity(ITopicProducer<string, OrderCompletedEvent> producer, ILogger<PaymentCompletedEventActivity> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, PaymentCompletedEvent> context, IBehavior<OrderSagaState, PaymentCompletedEvent> next)
        {
            await _producer.Produce(context.Message.CorrelationId.ToString(),
                new OrderCompletedEvent()
                {
                    CorrelationId = context.Message.CorrelationId,
                    CustomerId = context.Saga.CustomerId,
                    OrderId = context.Saga.OrderId.GetValueOrDefault(),
                },
                context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("OrderCompletedEvent published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, PaymentCompletedEvent, TException> context, IBehavior<OrderSagaState, PaymentCompletedEvent> next) where TException : Exception
        {
            // always call the next activity in the behavior
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("payment-completed-closed");
        }
    }
}
