using System;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Need.ApiGateway.Config;
using Need.ApiGateway.Models;

namespace Need.ApiGateway.Database
{
    public class ToiletContext : IToiletContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<Toilet> Toilets => _database.GetCollection<Toilet>("Toilet");

        public ToiletContext(IOptions<DatabaseConfig> config)
        {
            if (config?.Value == null)
                throw new ArgumentNullException(nameof(config));

            var client = new MongoClient(config.Value.ConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(config.Value.Name);
            }
        }
    }
}