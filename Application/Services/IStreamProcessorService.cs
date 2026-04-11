namespace Streaming.Producer.Application
{
    public interface IStreamProcessorService
    {
        Task<StreamProcessStatusDto> GetProcessProcessStatus(Guid processId);
        Task<StreamProcessDataDto> GetProcessProcessData(Guid processId);

        Task<StreamProcessStatusDto> CreateProcess(StreamProcessDto streamProcess);
        Task<StreamProcessStatusDto> Process(ProcessDto processDto);
    }
}
