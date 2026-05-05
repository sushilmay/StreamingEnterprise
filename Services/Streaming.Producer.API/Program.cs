using BuildingBlocks.MassTransit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Streaming.Producer.API.Exceptions.Handler;
using Streaming.Producer.Application;
using Streaming.Producer.Domain;
using Streaming.Producer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Read config values
var elasticUri = builder.Configuration["ElasticConfiguration:Uri"];
var appName = builder.Configuration["ApplicationName"] ?? "dotnet-app";

Console.WriteLine(elasticUri);
Console.WriteLine(appName);
var index = $"{appName}-logs-{builder.Environment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
    {
        AutoRegisterTemplate = true,
        NumberOfShards=2,
        NumberOfReplicas=1,
        //IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
        IndexFormat = index
    })
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

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

//Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddMongoDb(
        sp => new MongoClient(builder.Configuration["MongoDbSettings:ConnectionString"]),
        name: "mongodb",
        tags: new[] { "db", "data" },
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
    );



var app = builder.Build();
app.MapControllers();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
