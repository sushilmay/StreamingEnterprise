using BuildingBlocks.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Streaming.Producer.Application;
using Streaming.Producer.Domain;
using Streaming.Producer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


//Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

// Bind config
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register Mongo
builder.Services.AddSingleton<MongoContext>();

// Repository
builder.Services.AddScoped<IStreamProcessorRepository, StreamProcessorRepository>();

// Service
builder.Services.AddScoped<IStreamProcessorService, StreamProcessorService>();

// Controllers
builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddMongoDb(
        sp => new MongoClient(builder.Configuration["MongoDbSettings:ConnectionString"]),
        name: "mongodb",
        tags: new[] { "db", "data" },
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
    );
var app = builder.Build();
app.MapControllers();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
