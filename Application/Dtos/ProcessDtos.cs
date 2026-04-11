namespace Streaming.Producer.Application
{
    public record StreamProcessDto(int[] Data);
    public record ProcessDto(Guid ProcessId, int[] Data);
    public record StreamProcessStatusDto(Guid ProcessId, string ProcessStatus);
    public record StreamProcessDataDto(Guid ProcessId, int[] Data, int[] OutputData, string ProcessStatus);
}
