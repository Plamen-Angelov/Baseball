using Baseball.Common.ViewModels;

namespace Baseball.Core.Contracts
{
    public interface IBatMaterialService
    {
        IEnumerable<BatMaterialViewModel> GetAllBatMaterials();

        //Task<int> GetMaterialIdAsync(string materialName);

        Task<string> GetMaterialNameByIdAsync(int materialId);

    }
}
