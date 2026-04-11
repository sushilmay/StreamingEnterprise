namespace Streaming.Producer.API.Models
{
    public record StreamProcessRequest(int[] Data);
    public record StreamProcessStatusResponse(Guid ProcessId, string ProcessStatus);

    public record StreamProcessResponse(Guid ProcessId, int[] Data, int[] OutputData, string ProcessStatus);
}
