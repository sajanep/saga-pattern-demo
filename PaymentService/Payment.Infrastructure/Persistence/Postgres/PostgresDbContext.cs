using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using Payment.Infrastructure.Persistence.Postgres.EntityConfigurations;

namespace Payment.Infrastructure.Persistence.Postgres
{
    public class PostgresDbContext:DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) 
        {
        }

        public DbSet<PaymentRecord> PaymentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply PaymentConfiguration
            modelBuilder.ApplyConfiguration(new PaymentRecordConfiguration());
        }
    }
}
