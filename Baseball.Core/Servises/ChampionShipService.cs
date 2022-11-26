using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class ChampionShipService : IChampionShipService
    {
        private readonly IRepository repository;

        public ChampionShipService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<ChampionShipNameViewModel>> GetAllChampionShipNamesAsync()
        {
            return await repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false)
                .Select(c => new ChampionShipNameViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
