using DotNetCore.CAP;
using Payment.Application;
using SharedUtils;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace Payment.Api.MessageSubscribers
{
    public class PaymentMessageSubscribers:IPaymentMessageSubscribers, ICapSubscribe
    {
        private readonly ILogger<PaymentMessageSubscribers> _logger;
        private readonly ICapPublisher _capPublisher;
        private readonly IPaymentService _paymentService;

        public PaymentMessageSubscribers(ILogger<PaymentMessageSubscribers> logger, 
            ICapPublisher capPublisher,
            IPaymentService paymentService) 
        {
            _logger = logger;
            _capPublisher = capPublisher;
            _paymentService = paymentService;
        }

        [CapSubscribe(TopicNames.CompletePayment)]
        public async Task HandleCompletePaymentMessage(CompletePaymentMessage completePaymentMessage)
        {
            if(!string.IsNullOrEmpty(completePaymentMessage.CustomerId) && !string.IsNullOrEmpty(completePaymentMessage.PaymentAccountId))
            {
                if (completePaymentMessage.CustomerId == "test@test.com")
                {
                    _capPublisher.Publish(TopicNames.PaymentFailed, new PaymentFailedEvent
                    {
                        CorrelationId = completePaymentMessage.CorrelationId,
                        ErrorMessage = "Payment failed",
                        OrderItemList = completePaymentMessage.OrderItemList
                    });
                    return;
                }

                await _paymentService.ProcessPayment(new PaymentProcessRequestDto()
                {
                    PaymentAccountId = completePaymentMessage.PaymentAccountId,
                    UserId = completePaymentMessage.CustomerId,
                    Amount = completePaymentMessage.TotalPrice,
                });
                _capPublisher.Publish(TopicNames.PaymentCompleted, new PaymentCompletedEvent
                {
                    CorrelationId = completePaymentMessage.CorrelationId,
                });
                _logger.LogInformation("Payment successfull. {MessageTotalPrice}$ was withdrawn from user with Id= {MessageCustomerId} and correlation Id={MessageCorrelationId}",
                   completePaymentMessage.TotalPrice, completePaymentMessage.CustomerId, completePaymentMessage.CorrelationId);
            }
            else
            {
                _logger.LogError("Customer Id or Payment Id is missing for the correlation Id={MessageCorrelationId}", completePaymentMessage.CorrelationId);
            }
        }
    }
}
