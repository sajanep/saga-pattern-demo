using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;

namespace Order.Infrastructure.Persistence.Postgres.EntityConfigurations
{
 
    public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            // Table mapping
            builder.ToTable("Orders");

            // Primary key
            builder.HasKey(o => o.Id);

            // Properties
            builder.Property(o => o.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Auto-incrementing ID

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasMaxLength(50); // Assuming a max length of 50 characters

            builder.Property(o => o.PaymentAccountId)
                .IsRequired()
                .HasMaxLength(50); // Assuming a max length of 50 characters

            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>(); // Maps enum to an integer

            builder.Property(o => o.ErrorMessage)
                .HasMaxLength(500); // Limit error messages to 500 characters
        }
    }
}
