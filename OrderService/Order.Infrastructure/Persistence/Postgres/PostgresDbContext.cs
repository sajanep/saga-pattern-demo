using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using Order.Infrastructure.Persistence.Postgres.EntityConfigurations;


namespace Order.Infrastructure.Persistence.Postgres
{
    public class PostgresDbContext:DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) 
        {
        }

        public DbSet<OrderEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        }
    }
}
