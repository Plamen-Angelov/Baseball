using Baseball.Common.ViewModels.ChampionShipViewModels;

namespace Baseball.Core.Contracts
{
    public interface IChampionShipService
    {
        Task<List<ChampionShipNameViewModel>> GetAllChampionShipNamesAsync();
    }
}
