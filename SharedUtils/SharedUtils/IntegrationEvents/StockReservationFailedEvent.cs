using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedUtils.IntegrationEvents.Base;

namespace SharedUtils.IntegrationEvents
{
    public class StockReservationFailedEvent:IntegrationEvent
    {
        public string ErrorMessage { get; set; }
    }
}
