using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Core.Contracts
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamViewModel>> GetAllAsync();

        Task AddAsync(AddTeamModel model);

        Task<TeamDetailsViewModel> GetDetailsAsync(int id);

        Task<EditTeamViewModel?> GetByIdAsync(int id);

        Task<Team> GetEntityByIdAsync(int id);

        Task UpdateAsync(int id, EditTeamViewModel model);

        Task DeleteAsync(int id);

        Task<List<TeamNameViewModel>> GetAllTeamNamesAsync();

        Task<int> GetWinsAsync(Team team);

        Task<int> GetLosesAsync(Team team);
    }
}
