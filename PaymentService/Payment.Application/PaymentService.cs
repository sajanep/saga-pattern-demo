using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Infrastructure.Persistence.Postgres;

namespace Payment.Application
{
    public class PaymentService : IPaymentService
    {
        private readonly PostgresDbContext _dbContext;
        public PaymentService(PostgresDbContext postgresDbContext)
        {
            _dbContext = postgresDbContext;
        }

        public async Task ProcessPayment(PaymentProcessRequestDto paymentRequestDto)
        {
            // Call external payment gateway API and perform the transaction 
            var record = new PaymentRecord();
            record.PaymentDate = DateTime.UtcNow;
            record.UserId = paymentRequestDto.UserId;
            record.Amount = paymentRequestDto.Amount;

            _dbContext.PaymentRecords.Add(record);
            await _dbContext.SaveChangesAsync();

            return;
        }

        public async Task<List<PaymentGetResponseDto>> GetPaymentRecords(string userId)
        {
            var paymentGetResponses = new List<PaymentGetResponseDto>();
            var paymentRecords = _dbContext.PaymentRecords.Where(x => x.UserId == userId).ToList();
            if (paymentRecords != null && paymentRecords.Count > 0)
            {
                foreach (var paymentRecord in paymentRecords)
                {
                    var paymentGetResponse = new PaymentGetResponseDto()
                    {
                        Id = paymentRecord.Id,
                        PaymentDate = paymentRecord.PaymentDate,
                        UserId = paymentRecord.UserId,
                        Amount = paymentRecord.Amount,
                    };
                    paymentGetResponses.Add(paymentGetResponse);
                }
            }
            return paymentGetResponses;
        }
    }
}
