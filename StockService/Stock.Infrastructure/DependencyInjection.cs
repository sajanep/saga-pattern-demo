using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Infrastructure.Persistence.Postgres;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostgresDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("PostgresSql");
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
