using SharedUtils.Messages;

namespace Payment.Api.MessageSubscribers
{
    public interface IPaymentMessageSubscribers
    {
        Task HandleCompletePaymentMessage(CompletePaymentMessage completePaymentMessage);
    }
}
