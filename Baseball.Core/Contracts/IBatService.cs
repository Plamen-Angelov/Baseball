using Baseball.Common.ViewModels.BatViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatService
    {
        Task<IEnumerable<BatViewModel>> GetAllAsync();

        Task AddAsync(AddBatViewModel model);

        Task<AddBatViewModel> GetByIdAsync(int id);

        Task UpdateAsync(int id, AddBatViewModel model);

        Task DeleteAsync(int id);
    }
}
