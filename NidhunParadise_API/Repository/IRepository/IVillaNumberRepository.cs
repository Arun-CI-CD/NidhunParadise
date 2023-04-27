using NidhunParadise_API.Model;
using System.Linq.Expressions;

namespace NidhunParadise_API.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entity);
    }
}
