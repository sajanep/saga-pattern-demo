using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.Messages.Base
{
    public abstract class BaseMessage
    {
        public Guid CorrelationId { get; set; }
    }
}
