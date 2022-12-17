using Baseball.Common.ViewModels.PlayerViewModels;

namespace Baseball.Core.Contracts
{
    public interface IPlayerServicece
    {
        Task<IEnumerable<PlayerViewModel>> GetAllAsync(string? teamName = null, string? searchText = null, PlayerSorting? sorting = null);

        Task AddAsync(AddPlayerViewModel model);

        Task<AddPlayerViewModel> GetByIdAsync(int id);

        Task<PlayerViewModel> GetPlayerByIdAsync(int id);

        Task UpdateAsync(int id, AddPlayerViewModel model);

        Task DeleteAsync(int id);

        Task AddToTeamAsync(int playerId, int teamId);

        Task<int?> MakePlayerFreeAgentAsync(int playerId);
    }
}
