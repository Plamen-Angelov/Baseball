using Baseball.Common.ViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatService
    {
        IEnumerable<BatViewModel> GetAll();

        Task AddAsync(BatViewModel model);
    }
}
