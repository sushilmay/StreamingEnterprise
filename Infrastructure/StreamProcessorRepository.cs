using MongoDB.Driver;
using Streaming.Producer.Domain;


namespace Streaming.Producer.Infrastructure
{
    public class StreamProcessorRepository : IStreamProcessorRepository
    {
        private readonly MongoContext _context;

        public StreamProcessorRepository(MongoContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Process process)
        {
            var processData = new ProcessData
            {
                ProcessId = process.ProcessId,
                Input = process.InputData,
                Status = process.Status,
            };
            await _context.StreamCollection.InsertOneAsync(processData);
        }
        public async Task UpdateAsync(Process process)
        {
            var filter = Builders<ProcessData>.Filter
                .Eq(x => x.ProcessId, process.ProcessId);

            var update = Builders<ProcessData>.Update
                .Set(x => x.Input, process.InputData)
                .Set(x => x.Output, process.OutputData)
                .Set(x => x.Status, "Completed")
                .Set(x => x.ProcessedAt, DateTime.UtcNow);

            await _context.StreamCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Process?> GetByIdAsync(Guid id)
        {
            var processData = await _context.StreamCollection.Find(x => x.ProcessId == id).FirstOrDefaultAsync();
            if(processData != null)
            {
                return new Process
                {
                    ProcessId = processData.ProcessId,
                    InputData = processData.Input,
                    OutputData = processData.Output,
                    Status = processData.Status,
                };
            }
            return null;
        }
    }
}
