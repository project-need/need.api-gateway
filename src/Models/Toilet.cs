using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Need.ApiGateway.Models
{
    public class Toilet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Owner { get; set; }

        public string Location { get; set; }
    }
}