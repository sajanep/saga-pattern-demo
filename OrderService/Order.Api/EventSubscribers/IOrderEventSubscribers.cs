using SharedUtils.IntegrationEvents;

namespace Order.Api.EventSubscribers
{
    public interface IOrderEventSubscribers
    {
        Task HandleOrderCompleted(OrderCompletedEvent orderCompletedEvent);

        Task HandleOrderFailed(OrderFailedEvent orderFailedEvent);
    }
}
