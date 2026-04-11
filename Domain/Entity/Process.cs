namespace Streaming.Producer.Domain
{
    public class Process
    {
        public Guid ProcessId { get; set; }
        public int[] InputData { get; set; }
        public int[]? OutputData { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }

}
