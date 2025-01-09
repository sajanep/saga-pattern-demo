using System.Text.Json;
using DotNetCore.CAP.Internal;
using DotNetCore.CAP.Messages;
using Microsoft.Extensions.Configuration;
using Payment.Api.MessageSubscribers;
using Payment.Application;
using Payment.Infrastructure;
using Payment.Infrastructure.Persistence.Postgres;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureDI(builder.Configuration);
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMessageSubscribers, PaymentMessageSubscribers>();
builder.Services.AddCap(options =>
{
    options.UseEntityFramework<PostgresDbContext>(); // Required for Entity Framework Core
    options.UsePostgreSql(builder.Configuration.GetConnectionString("PostgresSql")); // PostgreSQL as persistence store
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
