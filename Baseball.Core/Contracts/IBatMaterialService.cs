using Baseball.Common.ViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatMaterialService
    {
        IEnumerable<BatMaterialViewModel> GetAllBatMaterials();
    }
}
