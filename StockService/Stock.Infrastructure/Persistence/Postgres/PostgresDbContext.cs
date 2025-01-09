using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Persistence.Postgres.EntityConfigurations;


namespace Stock.Infrastructure.Persistence.Postgres
{
    public class PostgresDbContext:DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) 
        {
        }

        public DbSet<StockEntity> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new StockEntityConfiguration());
        }
    }
}
