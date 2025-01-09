using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.IntegrationEvents.Base
{
    public abstract class IntegrationEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
