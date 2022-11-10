using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Infrastructure.Repository
{
    public interface IBatMaterialRepository
    {
        Task<IEnumerable<BatMaterial>> GetAll();

        //Task<int> GetMaterialIdAsync(string materialName);

        Task<string> GetMaterialNameByIdAsync(int materialId);
    }
}
