
using Baseball.Infrastructure.Data.Entities;

namespace Baseball.Infrastructure.Repository
{
    public interface IRepository : IDisposable
    {


        Task AddAsync<T>(T entity) where T : class;

        IQueryable<T> GetAll<T>() where T : class;

        void UpdateAsync<T>(T entity) where T : class;

        Task<int> SaveChangesAsync();
    }
}
