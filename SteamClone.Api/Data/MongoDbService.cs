using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SteamClone.Api.Models;

namespace SteamClone.Api.Data
{
    public class MongoDbService
    {
        public IMongoDatabase Database { get; }
        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            Database = client.GetDatabase(settings.Value.DatabaseName);

        }
    }
}
