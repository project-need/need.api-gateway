using System.Threading.Tasks;

namespace Need.ApiGateway.Database
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(string id);

        Task<string> AddAsync(T entity);

        Task UpdateAsync(string id, T entity);

        Task DeleteAsync(string id);
    }
}