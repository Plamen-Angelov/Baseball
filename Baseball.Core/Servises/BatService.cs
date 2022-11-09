using Baseball.Common.ViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;

namespace Baseball.Core.Servises
{
    public class BatService : IBatService
    {
        private readonly IRepository<Bat> repository;
        //private readonly IBatMaterialService batMaterialService;

        public BatService(IRepository<Bat> repository)
        {
            this.repository = repository;
            //this.batMaterialService = batMaterialService;
        }

        public Task AddAsync(BatViewModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BatViewModel> GetAll()
        {
            var bats = repository.GetAll()
                .Where(b => b.IsDeleted == false)
                .Select(b => new BatViewModel()
                {
                    Material = b.BatMaterial.Name,
                    Size = b.Size,
                    Brand = b.Brand
                })
                .ToList();

            return bats;
        }

        //public IEnumerable<BatMaterialViewModel> GetAllBatMaterials()
        //{
        //   var materials = batMaterialService.GetAllBatMaterials().ToList();

        //    return materials;
        //}
    }
}
