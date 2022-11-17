using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class BatService : IBatService
    {
        private readonly IRepository repository;
        private readonly IBatMaterialService batMaterialService;

        public BatService(IRepository repository, IBatMaterialService service)
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
            var bats = repository.GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .Where(b => b.IsDeleted == false)
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

        public AddBatViewModel GetById(int id)
        {
            var bat = repository
                .GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .FirstOrDefault(b => b.Id == id);

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
            var bat = repository
                .GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .FirstOrDefault(b => b.Id == id);

            if(bat == null || bat.IsDeleted == true)
            {
                throw new ArgumentException("Bat was not found");
            }

            bat.Brand = model.Brand;
            bat.Size = model.Size;
            bat.BatMaterialId = model.MaterialId;

            repository.UpdateAsync(bat);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bat = repository
                .GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .FirstOrDefault(b => b.Id == id);

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
