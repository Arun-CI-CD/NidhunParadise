using NidhunParadise_API.Model;
using System.Linq.Expressions;

namespace NidhunParadise_API.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entity);
    }
}
