using BuildingBlocks.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Streaming.Producer.Domain;
using static MassTransit.ValidationResultExtensions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Process = Streaming.Producer.Domain.Process;

namespace Streaming.Producer.Application
{
    public class StreamProcessorService : IStreamProcessorService
    {
        private readonly IStreamProcessorRepository _streamProcessorRepository;
        private readonly IPublishEndpoint _publish;
        private readonly ILogger<StreamProcessorService> _logger;
        public StreamProcessorService(IStreamProcessorRepository streamProcessorRepository, IPublishEndpoint publish, ILogger<StreamProcessorService> logger)
        {
            _streamProcessorRepository = streamProcessorRepository;
            _publish = publish;
            _logger = logger;
        }

        public async Task<StreamProcessStatusDto> GetProcessProcessStatus(Guid processId)
        {
            var result= await _streamProcessorRepository.GetByIdAsync(processId);
            if (result != null){
                return new StreamProcessStatusDto(result.ProcessId, result.Status);
            }
            return null;
        }
        public async Task<StreamProcessDataDto> GetProcessProcessData(Guid processId)
        {
            var result = await _streamProcessorRepository.GetByIdAsync(processId);
            if (result != null)
            {
                return new StreamProcessDataDto(result.ProcessId, result.InputData, result.OutputData, result.Status);
            }
            return null;
        }
        public async Task<StreamProcessStatusDto> CreateProcess(StreamProcessDto streamProcess)
        {
            _logger.LogInformation("Start Executing CreateProcess Method of StreamProcessorService");
            var process = new Process
            {
                ProcessId = Guid.NewGuid(),
                InputData = streamProcess.Data,
                Status = "Pending",
            };
            //DB Save
            await _streamProcessorRepository.AddAsync(process);
            //Published
            await _publish.Publish(new StreamRequestEvent(process.ProcessId, process.InputData));
            return new StreamProcessStatusDto(process.ProcessId, process.Status);
        }

        public async Task<StreamProcessStatusDto> Process(ProcessDto processDto)
        {
            //Procees Data 
            var result = processDto.Data.Where(x => x != 0).Concat(processDto.Data.Where(x => x == 0)).ToArray();

            var process = new Process
            {
                ProcessId = processDto.ProcessId,
                InputData = processDto.Data,
                OutputData = result,
                Status = "Completed",
            };
            //DB Save
            await _streamProcessorRepository.UpdateAsync(process);
            return new StreamProcessStatusDto(process.ProcessId, process.Status);
        }
    }
}
