using Baseball.Common.ViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;


namespace Baseball.Core.Servises
{
    public class BatMaterialService : IBatMaterialService
    {
        private readonly IBatMaterialRepository repository;

        public BatMaterialService(IBatMaterialRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<BatMaterialViewModel> GetAllBatMaterials()
        {
            var materials = repository.GetAll()
                .Result
                .Select(m => new BatMaterialViewModel()
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToList();

            return materials;
        }

        //public Task<int> GetMaterialId(string materialName)
        //{
        //    return repository.GetMaterialId(materialName);
        //}
    }
}
