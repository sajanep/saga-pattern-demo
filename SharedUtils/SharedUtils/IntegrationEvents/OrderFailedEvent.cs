using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.IntegrationEvents.Base;

namespace SharedUtils.IntegrationEvents
{
    public class OrderFailedEvent:IntegrationEvent
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
