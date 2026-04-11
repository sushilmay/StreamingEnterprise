namespace Streaming.Producer.Application
{
    public interface IStreamProcessorService
    {
        Task<StreamProcessStatusDto> GetProcessStatus(Guid processId);
        Task<StreamProcessDataDto> GetProcessData(Guid processId);

        Task<StreamProcessStatusDto> CreateProcess(StreamProcessDto streamProcess);
        Task<StreamProcessStatusDto> Process(ProcessDto processDto);
    }
}
