using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Core.Contracts
{
    public interface IPlayerServicece
    {
        Task <IEnumerable<PlayerViewModel>> GetAllAsync();

        Task AddAsync(AddPlayerViewModel model);

        Task<AddPlayerViewModel> GetByIdAsync(int id);

        Task UpdateAsync(int id, AddPlayerViewModel model);

        Task DeleteAsync(int id);
    }
}
