using Confluent.Kafka;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities
{
    public class CreateOrderActivity : IStateMachineActivity<OrderSagaState, CreateOrderMessage>
    {
        private readonly ITopicProducer<string, OrderCreatedEvent> _producer;
        private readonly ILogger<CreateOrderActivity> _logger;

        public CreateOrderActivity(ITopicProducer<string, OrderCreatedEvent> producer, ILogger<CreateOrderActivity> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderSagaState, CreateOrderMessage> context, IBehavior<OrderSagaState, CreateOrderMessage> next)
        {
            await _producer.Produce(context.Message.CorrelationId.ToString(),
              new OrderCreatedEvent()
              {
                  CorrelationId = context.Message.CorrelationId,
                  OrderId = context.Message.OrderId,
                  OrderItemList = context.Message.OrderItemList
              },
              context.CancellationToken).ConfigureAwait(false);
            _logger.LogInformation("OrderCreatedEvent published in OrderStateMachine for CorrelationId: {CorrelationId} ", context.Saga.CorrelationId);

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderSagaState, CreateOrderMessage, TException> context, IBehavior<OrderSagaState, CreateOrderMessage> next) where TException : Exception
        {
            // always call the next activity in the behavior
            return next.Faulted(context);
        }

        public void Probe(ProbeContext context)
        {
            context.CreateScope("create-order-closed");
        }

    }
}
