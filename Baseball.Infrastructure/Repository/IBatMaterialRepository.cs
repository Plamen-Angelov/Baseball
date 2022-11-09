using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Infrastructure.Repository
{
    public interface IBatMaterialRepository
    {
        Task<IEnumerable<BatMaterial>> GetAll();
    }
}
