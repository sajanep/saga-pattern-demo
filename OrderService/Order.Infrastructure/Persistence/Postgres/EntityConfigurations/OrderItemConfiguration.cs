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
 
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Table mapping
            builder.ToTable("OrderItems");

            // Primary key
            builder.HasKey(oi => oi.Id);

            // Properties
            builder.Property(oi => oi.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Auto-incrementing ID

            builder.Property(oi => oi.OrderId)
                .IsRequired();

            builder.Property(oi => oi.ProductId)
                .IsRequired();

            builder.Property(oi => oi.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // Setting precision for the price

            builder.Property(oi => oi.Count)
                .IsRequired();

            // Relationships
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItemList) // One-to-many relationship
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Delete OrderItems when Order is deleted
        }
    }
}
