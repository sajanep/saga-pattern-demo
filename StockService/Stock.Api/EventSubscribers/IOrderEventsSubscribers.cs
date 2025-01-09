using SharedUtils.IntegrationEvents;

namespace Stock.Api.EventSubscribers
{
    public interface IOrderEventsSubscribers
    {
        Task HandleOrderCreated(OrderCreatedEvent orderCreatedEvent);
    }
}
