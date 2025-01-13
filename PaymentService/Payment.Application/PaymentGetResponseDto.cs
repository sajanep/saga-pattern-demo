using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application
{
    public class PaymentGetResponseDto
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string AccountNumber{ get; set; }

        public string UserId { get; set; } // Foreign key for User (assuming a User table exists)
    }
}
