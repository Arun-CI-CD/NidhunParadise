using NidhunParadise_API.Model;
using System.Linq.Expressions;

namespace NidhunParadise_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateVillaAsync(T entity);
        Task RemoveVillaAsync(T entity);
        Task SaveAsync();
    }
}
