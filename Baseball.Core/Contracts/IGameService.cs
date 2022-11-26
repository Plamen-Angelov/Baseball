using Baseball.Common.ViewModels.GameViewModels;

namespace Baseball.Core.Contracts
{
    public interface IGameService
    {
        Task<IEnumerable<GameViewModel>> GetAllAsync();

        Task AddAsync(AddGameViewModel model);
    }
}
