
using BuildingBlocks.MassTransit;
using Streaming.Producer.Application;
using Streaming.Producer.Domain;
using Streaming.Producer.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<StreamConsumer>();

//    x.UsingRabbitMq((ctx, cfg) =>
//    {
//        cfg.Host("rabbitmq");
//        cfg.ReceiveEndpoint("stream-queue", e =>
//        {
//            e.ConfigureConsumer<StreamConsumer>(ctx);
//        });
//    });
//});
var assembly = typeof(Program).Assembly;
builder.Services.AddMessageBroker(builder.Configuration, assembly);

// Bind config
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Register Mongo
builder.Services.AddSingleton<MongoContext>();

// Repository
builder.Services.AddScoped<IStreamProcessorRepository, StreamProcessorRepository>();

// Service
builder.Services.AddScoped<IStreamProcessorService, StreamProcessorService>();

var app = builder.Build();
app.Run();


