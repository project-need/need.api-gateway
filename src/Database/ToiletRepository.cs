using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Need.ApiGateway.Models;

namespace Need.ApiGateway.Database
{
    public class ToiletRepository : IRepository<Toilet>
    {
        private readonly IToiletContext _context;

        public ToiletRepository(IToiletContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> AddAsync(Toilet toilet)
        {
            await _context.Toilets.InsertOneAsync(toilet);

            return toilet.Id;
        }

        public async Task DeleteAsync(string id)
        {
            await _context.Toilets.DeleteOneAsync(a => a.Id == id);
        }

        public async Task<Toilet> GetAsync(string id)
        {
            return await _context.Toilets.Find(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, Toilet toilet)
        {
            toilet.Id = id;

            await _context.Toilets.ReplaceOneAsync(
                a => a.Id == id, 
                toilet, 
                new ReplaceOptions{ IsUpsert = true });
        }
    }
}