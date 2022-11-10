using Baseball.Common.ViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;

namespace Baseball.Core.Servises
{
    public class BatService : IBatService
    {
        private readonly IRepository<Bat> repository;
        private readonly IBatMaterialService batMaterialService;

        public BatService(IRepository<Bat> repository, IBatMaterialService service)
        {
            this.repository = repository;
            this.batMaterialService = service;
        }

        public async Task AddAsync(AddBatViewModel model)
        {
            var bat = new Bat()
            {
                Brand = model.Brand,
                Size = model.Size,
                BatMaterialId = model.MaterialId
            };

            await repository.AddAsync(bat);
            await repository.SaveChangesAsync();
        }

        public IEnumerable<BatViewModel> GetAll()
        {
            var bats = repository.GetAllAsync()
                .Result
                .Select(b => new BatViewModel()
                {
                    id = b.Id,
                    Material = b.BatMaterial.Name,
                    Size = b.Size,
                    Brand = b.Brand
                })
                .ToList();

            return bats;
        }

        public async Task<AddBatViewModel> GetByIdAsync(int id)
        {
            var bat = await repository.GetByIdAsync(id);

            if (bat == null || bat.IsDeleted == true)
            {
                throw new ArgumentException("Bat was not found");
            }

            var batVieeModel = new AddBatViewModel()
            {
                Id = id,
                MaterialId = bat.BatMaterial.Id,
                Materials = batMaterialService.GetAllBatMaterials().ToList(),
                Size = bat.Size,
                Brand = bat.Brand
            };

            return batVieeModel;
        }

        public async Task UpdateAsync(int id, AddBatViewModel model)
        {
            var bat = await repository.GetByIdAsync(id);

            if (bat == null || bat.IsDeleted == true)
            {
                throw new ArgumentException("Bat was not found");
            }

            bat.Brand = model.Brand;
            bat.Size = model.Size;
            bat.BatMaterial.Name = await batMaterialService.GetMaterialNameByIdAsync(model.MaterialId);

            repository.UpdateAsync(bat);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bat = await repository.GetByIdAsync(id);

            if (bat == null || bat.IsDeleted == true)
            {
                throw new ArgumentException("Bat was not found");
            }

            bat.IsDeleted = true;

            repository.UpdateAsync(bat);
            await repository.SaveChangesAsync();
        }
    }
}
