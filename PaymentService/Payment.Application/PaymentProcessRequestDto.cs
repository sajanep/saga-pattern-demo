namespace Payment.Application
{
    public class PaymentProcessRequestDto
    {
        public string PaymentAccountId { get; set; }

        public decimal Amount { get; set; }

        public string UserId { get; set; } // Foreign key for User (assuming a User table exists)
    }
}
