using MassTransit;
using Microsoft.Extensions.Logging;
using OrchestrationService.Infrastructure.StateMachineInstances;
using OrchestrationService.Infrastructure.StateMachineInstances.CustomActivities;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace OrchestrationService.Infrastructure.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderSagaState>
    {
        private readonly ILogger<OrderStateMachine> _logger;
        // Commands
        private Event<CreateOrderMessage> CreateOrderMessage { get; set; }

        // Events
        public Event<StockReservedEvent> StockReservedEvent { get; set; }
        public Event<StockReservationFailedEvent> StockReservationFailedEvent { get; set; }
        public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }

        // States
        public State? OrderCreated { get; set; }
        public State? StockReserved { get; set; }
        public State? StockReservationFailed { get; set; }
        public State? PaymentCompleted { get; set; }
        public State? PaymentFailed { get; set; }

        public OrderStateMachine(ILogger<OrderStateMachine> logger)
        {
            _logger = logger;
            InstanceState(x => x.CurrentState);

            Event(() => CreateOrderMessage, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => StockReservedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => StockReservationFailedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => PaymentCompletedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Initially(
                When(CreateOrderMessage)
               .Then(context => { _logger.LogInformation("CreateOrderMessage received in OrderStateMachine for CorrelationId: {CorrelationId}", context.Saga.CorrelationId); })
               .Then(context =>
               {
                   context.Saga.CustomerId = context.Message.CustomerId;
                   context.Saga.OrderId = context.Message.OrderId;
                   context.Saga.CreatedDate = DateTime.UtcNow;
                   context.Saga.PaymentAccountNumber = context.Message.PaymentAccountId;
                   context.Saga.TotalPrice = context.Message.TotalPrice;
               })
               .Activity(x => x.OfType<CreateOrderActivity>())
               .TransitionTo(OrderCreated));

            During(OrderCreated,
                When(StockReservedEvent)
                    .Then(context => { _logger.LogInformation("StockReservedEvent received in OrderStateMachine for CorrelationId: {CorrelationId}", context.Saga.CorrelationId); })
                    .Activity(x => x.OfType<StockReservedEventActivity>())
                    .TransitionTo(StockReserved),

                When(StockReservationFailedEvent)
                    .Then(context => { _logger.LogInformation("StockReservationFailedEvent received in OrderStateMachine for CorrelationId: {CorrelationId}", context.Saga.CorrelationId); })
                    .Activity(x => x.OfType<StockReservationEventFailedActivity>())
                    .TransitionTo(StockReservationFailed));

            During(StockReserved,
                When(PaymentCompletedEvent)
                    .Then(context => { _logger.LogInformation("PaymentCompletedEvent received in OrderStateMachine for CorrelationId: {CorrelationId}", context.Saga.CorrelationId); })
                    .Activity(x => x.OfType<PaymentCompletedEventActivity>())
                    .TransitionTo(PaymentCompleted)
                    .Finalize(),

                When(PaymentFailedEvent)
                    .Then(context => { _logger.LogInformation("PaymentFailedEvent received in OrderStateMachine: for CorrelationId: {CorrelationId}", context.Saga.CorrelationId); })
                    .Activity(x => x.OfType<PaymentFailedEventActivity>())
                    .TransitionTo(PaymentFailed));

            // Delete finished saga instances from the repository
            SetCompletedWhenFinalized();
        }
    }
}
