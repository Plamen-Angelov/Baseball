using Baseball.Common.ViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatService
    {
        IEnumerable<BatViewModel> GetAll();

        Task AddAsync(AddBatViewModel model);

        Task<AddBatViewModel> GetByIdAsync(int id);

        Task UpdateAsync(int id, BatViewModel model);
    }
}
