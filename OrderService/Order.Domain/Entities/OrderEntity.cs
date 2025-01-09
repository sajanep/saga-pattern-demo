using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    public class OrderEntity
    {
        public OrderEntity()
        {
            OrderItemList = new List<OrderItem>();
        }
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string PaymentAccountId { get; set; }
        public OrderStatus Status { get; set; }
        public string? ErrorMessage { get; set; }
        public virtual List<OrderItem> OrderItemList { get; set; }
    }

    public enum OrderStatus
    {
        Pending = 0,
        Complete = 1,
        Fail = 2
    }
}
