using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Persistence.Postgres.EntityConfigurations
{
 
    public class StockEntityConfiguration : IEntityTypeConfiguration<StockEntity>
    {
        public void Configure(EntityTypeBuilder<StockEntity> builder)
        {
            // Table name (optional, otherwise EF will use pluralized name)
            builder.ToTable("Stocks");

            // Primary Key
            builder.HasKey(p => p.Id);

            // Property Configurations
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Auto-increment primary key

            builder.Property(e => e.ProductId)
             .IsRequired();

            builder.Property(e => e.Count)
                .IsRequired();
        }
    }
}
