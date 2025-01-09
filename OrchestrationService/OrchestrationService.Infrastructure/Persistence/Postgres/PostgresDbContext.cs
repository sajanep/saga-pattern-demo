using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using OrchestrationService.Infrastructure.StateMachineInstances;
using OrchestrationService.Infrastructure.StateMachineMaps;

namespace OrchestrationService.Infrastructure.Persistence.Postgres
{
    public class PostgresDbContext: SagaDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) 
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderSagaStateMap(); }
        }
        
    }
}
