using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;


namespace Baseball.Core.Servises
{
    public class BatMaterialService : IBatMaterialService
    {
        private readonly IRepository repository;

        public BatMaterialService(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<BatMaterialViewModel> GetAllBatMaterials()
        {
            var materials = repository.GetAll<BatMaterial>()
                .Where(m => m.IsDeleted == false)
                .Select(m => new BatMaterialViewModel()
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToList();

            return materials;
        }
    }
}
