using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Streaming.Producer.Infrastructure
{
        public class ProcessData
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid ProcessId { get; set; }

        public int[] Input { get; set; }

        public int[] Output { get; set; }

        public string? Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}
