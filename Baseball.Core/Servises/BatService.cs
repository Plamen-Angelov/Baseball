using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Baseball.Core.Servises
{
    public class BatService : IBatService
    {
        private readonly IRepository repository;
        private readonly IBatMaterialService batMaterialService;
        private readonly ILogger logger;

        public BatService(IRepository repository, IBatMaterialService batMaterialService, ILogger<BatService> logger)
        {
            this.repository = repository;
            this.batMaterialService = batMaterialService;
            this.logger = logger;
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

        public async Task<IEnumerable<BatViewModel>> GetAllAsync()
        {
            var bats = await repository.GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .Where(b => b.IsDeleted == false)
                .Select(b => new BatViewModel()
                {
                    id = b.Id,
                    Material = b.BatMaterial.Name,
                    Size = b.Size,
                    Brand = b.Brand
                })
                .ToListAsync();

            return bats;
        }

        public async Task<AddBatViewModel> GetByIdAsync(int id)
        {
            var bat = await repository
                .GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bat == null || bat.IsDeleted == true)
            {
                logger.LogError("Bat with id {0} does not exists or it is flaged deleted", id);
                throw new ArgumentException("Bat was not found");
            }

            var batViewModel = new AddBatViewModel()
            {
                Id = id,
                MaterialId = bat.BatMaterial.Id,
                Materials = (await batMaterialService.GetAllBatMaterialsAsync()).ToList(),
                Size = bat.Size,
                Brand = bat.Brand
            };

            return batViewModel;
        }

        public async Task UpdateAsync(int id, AddBatViewModel model)
        {
            var bat = repository
                .GetAll<Bat>()
                .Include(b => b.BatMaterial)
                .FirstOrDefault(b => b.Id == id);

            if(bat == null || bat.IsDeleted == true)
            {
                logger.LogError("Bat with id {0} does not exists or it is flaged deleted", id);
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
                logger.LogError("Bat with id {0} does not exists or it is flaged deleted", id);
                throw new ArgumentException("Bat was not found");
            }

            bat.IsDeleted = true;

            repository.UpdateAsync(bat);
            await repository.SaveChangesAsync();
        }
    }
}
