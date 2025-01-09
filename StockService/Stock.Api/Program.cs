using Stock.Application;
using Stock.Api.EventSubscribers;
using Stock.Infrastructure;
using Stock.Infrastructure.Persistence.Postgres;
using Stock.Infrastructure.Repository;
using Stock.Api.MessageSubscribers;
using DotNetCore.CAP.Internal;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IOrderEventsSubscribers, OrderEventsSubscribers>();
builder.Services.AddScoped<IStockMessageSubscribers, StockMessageSubscribers>();
builder.Services.AddCap(options =>
{
    options.UseEntityFramework<PostgresDbContext>(); // Required for Entity Framework Core
    options.UsePostgreSql(builder.Configuration.GetConnectionString("PostgresSql")); // PostgreSQL as persistence store
    // Use Redpanda as the event bus (via Kafka-compatible endpoint)
    options.UseKafka(opt =>
    {
        opt.Servers = builder.Configuration["Kafka:BootstrapServers"];
        opt.CustomHeadersBuilder = (kafkaResult, sp) => new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("cap-msg-id",
        sp.GetRequiredService<ISnowflakeId>().NextId().ToString()),
            new KeyValuePair<string, string>("cap-msg-name", kafkaResult.TopicPartition.Topic) };
    });
    options.FailedThresholdCallback = (failedInfo) =>
    {
        Console.WriteLine("Cap Failure: Failed Message Type {0} and Message {1}",
            failedInfo.MessageType.ToString(),
            JsonSerializer.Serialize(failedInfo.Message));
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
