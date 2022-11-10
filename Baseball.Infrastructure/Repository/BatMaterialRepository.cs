using Baseball.Infrastructure.Data;
using Baseball.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Infrastructure.Repository
{
    public class BatMaterialRepository : IBatMaterialRepository
    {
        private readonly BaseballDbContext context;

        public BatMaterialRepository(BaseballDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<BatMaterial>> GetAll()
        {
            var materials = await context.BatMaterials
                .Where(m => m.IsDeleted == false)
                .ToListAsync();

            return materials;
        }

        //public async Task<int> GetMaterialIdAsync(string materialName)
        //{
        //    return await context.BatMaterials
        //        .Where(m => m.Name == materialName)
        //        .Select(m => m.Id)
        //        .FirstOrDefaultAsync();
        //}

        public async Task<string?> GetMaterialNameByIdAsync(int materialId)
        {
            return await context.BatMaterials
               .Where(m => m.Id == materialId)
               .Select(m => m.Name)
               .FirstOrDefaultAsync();
        }
    }
}
