using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.Dtos;
using SharedUtils.IntegrationEvents.Base;

namespace SharedUtils.IntegrationEvents
{
    public class OrderCreatedEvent:IntegrationEvent
    {
        public OrderCreatedEvent() 
        {
            OrderItemList = new List<OrderItem>();
        }
        public List<OrderItem> OrderItemList { get; set; }
    }
}
