using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.Entities;

namespace Payment.Infrastructure.Persistence.Postgres.EntityConfigurations
{

    public class PaymentRecordConfiguration : IEntityTypeConfiguration<PaymentRecord>
    {
        public void Configure(EntityTypeBuilder<PaymentRecord> builder)
        {
            // Table name (optional, otherwise EF will use pluralized name)
            builder.ToTable("Payments");

            // Primary Key
            builder.HasKey(p => p.Id);

            // Property Configurations
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Auto-increment primary key

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // Precision of 18 digits and 2 decimal places

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.UserId)
                .IsRequired();
        }
    }
}
