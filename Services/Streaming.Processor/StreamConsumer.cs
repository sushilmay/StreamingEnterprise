using BuildingBlocks.Contracts.Events;
using MassTransit;
using Streaming.Producer.Application;

namespace Streaming.Processor
{
    public class StreamConsumer : IConsumer<StreamRequestEvent>
    {
        private readonly IStreamProcessorService _service;
        private readonly ILogger<StreamConsumer> _logger;
        public StreamConsumer(IStreamProcessorService service, ILogger<StreamConsumer> logger) { 
            this._service= service;
            this._logger = logger;
        }
        public Task Consume(ConsumeContext<StreamRequestEvent> context)
        {
            _logger.LogInformation("Start Executing Consume Method of StreamConsumer");
            _service.Process(new ProcessDto(context.Message.ProcessId,context.Message.Data));

            return Task.CompletedTask;
        }
    }
}
