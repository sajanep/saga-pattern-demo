using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.IntegrationEvents.Base;

namespace SharedUtils.IntegrationEvents
{
    public class OrderCompletedEvent:IntegrationEvent
    {
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
    }
}
