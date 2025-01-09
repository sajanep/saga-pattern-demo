using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application
{
    public interface IPaymentService
    {
        Task ProcessPayment(PaymentProcessRequestDto paymentRequestDto);

        Task<List<PaymentGetResponseDto>> GetPaymentRecords(string userId);
    }
}
