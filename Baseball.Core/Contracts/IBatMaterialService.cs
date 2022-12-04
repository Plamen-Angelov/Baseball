using Baseball.Common.ViewModels.BatViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatMaterialService
    {
        Task<IEnumerable<BatMaterialViewModel>> GetAllBatMaterialsAsync();

    }
}
