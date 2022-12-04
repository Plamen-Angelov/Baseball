using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class BatMaterialService : IBatMaterialService
    {
        private readonly IRepository repository;

        public BatMaterialService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<BatMaterialViewModel>> GetAllBatMaterialsAsync()
        {
            var materials = await repository.GetAll<BatMaterial>()
                .Where(m => m.IsDeleted == false)
                .Select(m => new BatMaterialViewModel()
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToListAsync();

            return materials;
        }
    }
}
