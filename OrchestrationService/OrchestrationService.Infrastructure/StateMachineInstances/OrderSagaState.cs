using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using MassTransit;

namespace OrchestrationService.Infrastructure.StateMachineInstances
{
    public class OrderSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public string? CurrentState { get; set; }

        public int? OrderId { get; set; }

        public string? CustomerId { get; set; }

        public string? PaymentAccountNumber { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime? CreatedDate { get; set; }

        // If using Optimistic concurrency, this property is required
        public uint RowVersion { get; set; }
    }
}
