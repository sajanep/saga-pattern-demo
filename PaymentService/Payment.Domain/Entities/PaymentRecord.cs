using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Entities
{
    public class PaymentRecord
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public decimal Amount { get; set; }
        
        public DateTime PaymentDate { get; set; }

        public string UserId { get; set; } // Foreign key for User (assuming a User table exists)
    }
}
