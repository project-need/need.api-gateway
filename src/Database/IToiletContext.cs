using MongoDB.Driver;
using Need.ApiGateway.Models;

namespace Need.ApiGateway.Database
{
    public interface IToiletContext
    {
        IMongoCollection<Toilet> Toilets { get; }
    }
}