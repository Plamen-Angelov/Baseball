using Baseball.Common.ViewModels.BatViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatService
    {
        IEnumerable<BatViewModel> GetAll();

        Task AddAsync(AddBatViewModel model);

        AddBatViewModel GetById(int id);

        Task UpdateAsync(int id, AddBatViewModel model);

        Task DeleteAsync(int id);
    }
}
