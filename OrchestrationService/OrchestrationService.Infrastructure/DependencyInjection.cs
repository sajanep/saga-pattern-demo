using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Confluent.Kafka;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchestrationService.Infrastructure.Persistence.Postgres;
using OrchestrationService.Infrastructure.StateMachineInstances;
using OrchestrationService.Infrastructure.StateMachines;
using SharedUtils;
using SharedUtils.IntegrationEvents;
using SharedUtils.Messages;

namespace OrchestrationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Mass-transit Saga persistence EF Configuration
            services.AddMassTransit(x =>
            {
                // Configure the transport
                x.UsingInMemory((context, busCfg) =>
                {
                    // Configure JSON serializer options for PascalCase
                    busCfg.ConfigureJsonSerializerOptions(options =>
                    {
                        // Remove any default converters for decimal type, since by default decimal is converted to string during serialization
                        var decimalConverter = options.Converters
                            .FirstOrDefault(c => c.CanConvert(typeof(decimal)));

                        if (decimalConverter != null)
                        {
                            options.Converters.Remove(decimalConverter);
                        }

                        // Add custom decimal converter to ensure it's serialized as a number
                        options.Converters.Add(new DecimalAsNumberJsonConverter());
                        options.PropertyNamingPolicy = null; // Use PascalCase
                        options.DictionaryKeyPolicy = null;  // Use PascalCase for dictionary keys
                        options.PropertyNameCaseInsensitive = true;   // Allow case-insensitive deserialization
                        // Ensure decimal is serialized as a number
                        options.Converters.Add(new DecimalAsNumberJsonConverter());
                        return options;
                    });

                    busCfg.ConfigureEndpoints(context);
                });

                x.AddRider(rider =>
                {
                    rider.AddSagaStateMachine<OrderStateMachine, OrderSagaState>()
                        .EntityFrameworkRepository(r =>
                        {
                            r.ConcurrencyMode = ConcurrencyMode.Optimistic; // or use Pessimistic, which does not require RowVersion

                            r.AddDbContext<DbContext, PostgresDbContext>((provider, builder) =>
                            {
                                builder.UseNpgsql(configuration.GetConnectionString("PostgresSql"), m =>
                                {
                                    m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                    m.MigrationsHistoryTable($"__{nameof(PostgresDbContext)}");
                                });
                            });

                            //This line is added to enable PostgreSQL features
                            r.UsePostgres();
                        });

                    rider.UsingKafka(new ClientConfig() { 
                        BootstrapServers = configuration["Kafka:BootstrapServers"],
                        //SaslUsername = "", SaslPassword = "", SaslMechanism = SaslMechanism.Plain, 
                        SecurityProtocol = SecurityProtocol.Plaintext,  
                        ApiVersionRequest = true }, 
                        (context, kafka ) => {
                            
                            // Configure saga endpoints for Kafka topics
                            kafka.TopicEndpoint<CreateOrderMessage>(TopicNames.CreateOrder, TopicNames.CreateOrder + "-group", e =>
                            {
                                e.ConfigureSaga<OrderSagaState>(context);
                            });

                            kafka.TopicEndpoint<PaymentCompletedEvent>(TopicNames.PaymentCompleted, TopicNames.PaymentCompleted + "-group", e =>
                            {
                                e.ConfigureSaga<OrderSagaState>(context);
                            });

                            kafka.TopicEndpoint<PaymentFailedEvent>(TopicNames.PaymentFailed, TopicNames.PaymentFailed + "-group", e =>
                            {
                                e.ConfigureSaga<OrderSagaState>(context);
                            });

                            kafka.TopicEndpoint<StockReservedEvent>(TopicNames.StockReserved, TopicNames.StockReserved + "-group", e =>
                            {
                                e.ConfigureSaga<OrderSagaState>(context);
                            });

                            kafka.TopicEndpoint<StockReservationFailedEvent>(TopicNames.StockReservationFailed, TopicNames.StockReservationFailed + "-group", e =>
                            {
                                e.ConfigureSaga<OrderSagaState>(context);
                            });
                        });

                    rider.AddProducer<string, OrderCreatedEvent>(TopicNames.OrderCreated);
                    rider.AddProducer<string, CompletePaymentMessage>(TopicNames.CompletePayment);
                    rider.AddProducer<string, OrderCompletedEvent>(TopicNames.OrderCompleted);
                    rider.AddProducer<string, OrderFailedEvent>(TopicNames.OrderFailed);
                    rider.AddProducer<string, StockRollBackMessage>(TopicNames.StockRollback);
                });
            });
            
            return services;
        }

        public class DecimalAsNumberJsonConverter : JsonConverter<decimal>
        {
            public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String && decimal.TryParse(reader.GetString(), out var value))
                {
                    return value;
                }
                return reader.GetDecimal();
            }

            public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            {
                writer.WriteNumberValue(value); // Serialize as a number
            }
        }
    }
}
