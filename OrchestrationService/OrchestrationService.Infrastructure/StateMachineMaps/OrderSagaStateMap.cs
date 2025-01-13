using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrchestrationService.Infrastructure.StateMachineInstances;

namespace OrchestrationService.Infrastructure.StateMachineMaps
{
    public class OrderSagaStateMap: SagaClassMap<OrderSagaState>
    {
        protected override void Configure(EntityTypeBuilder<OrderSagaState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(255);
            entity.Property(x => x.OrderId);
            entity.Property(x => x.TotalPrice);
            entity.Property(x => x.CreatedDate);
            entity.Property(x => x.CustomerId).HasMaxLength(255);
            entity.Property(x => x.PaymentAccountNumber).HasMaxLength(255);

            // If using Optimistic concurrency, otherwise remove this property
            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .IsRowVersion();
        }
    }
}
