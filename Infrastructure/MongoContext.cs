using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Streaming.Producer.Infrastructure
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IOptions<MongoDbSettings> settings)
        {
            var mongoSettings = settings.Value;

            var client = new MongoClient(mongoSettings.ConnectionString);
            _database = client.GetDatabase(mongoSettings.DatabaseName);
        }

        public IMongoCollection<ProcessData> StreamCollection =>
            _database.GetCollection<ProcessData>("ProcessData");
    }
}