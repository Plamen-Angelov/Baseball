using Baseball.Common.ViewModels.ChampionShipViewModels;

namespace Baseball.Core.Contracts
{
    public interface IChampionShipService
    {
        Task<List<ChampionShipNameViewModel>> GetAllChampionShipNamesAsync();

        Task<List<ChampionShipViewModel>> GetAllAsync();

        Task AddAsync(AddChampionShipViewModel model);

        Task<EditChampionShipViewModel> GetByIdAsync(int id);

        Task UpdateAsync(int id, EditChampionShipViewModel model);
    }
}
