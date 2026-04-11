using BuildingBlocks.Contracts.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using Streaming.Producer.Application.Exceptions;
using Streaming.Producer.Domain;
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

        public async Task<StreamProcessStatusDto> GetProcessStatus(Guid processId)
        {
            var result= await _streamProcessorRepository.GetByIdAsync(processId);
            return result is null ? throw new ProcessIdNotFoundException(processId.ToString()) : new StreamProcessStatusDto(result.ProcessId, result.Status);
        }
        public async Task<StreamProcessDataDto> GetProcessData(Guid processId)
        {
            var result = await _streamProcessorRepository.GetByIdAsync(processId);
            return result is null ? throw new ProcessIdNotFoundException(processId.ToString()) :
                new StreamProcessDataDto(result.ProcessId, result.InputData, result.OutputData, result.Status);

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
            var result = SwapZero(processDto.Data);

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

        private int[] SwapZero(int[] data)
        {
            var result = new int[data.Length];
            int index = 0;

            foreach (var x in data)
                if (x != 0)
                    result[index++] = x;

            return result;
        }
    }
}
