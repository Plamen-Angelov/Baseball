using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Core.Contracts
{
    public interface IGameService
    {
        Task<IEnumerable<GameViewModel>> GetAllAsync();

        Task AddAsync(AddGameViewModel model);

        Task<EditGameViewModel?> GetByIdAsync(int id);

        Task UpdateAsync(int id, EditGameViewModel model);

        Task DeleteAsync(int id);
    }
}
