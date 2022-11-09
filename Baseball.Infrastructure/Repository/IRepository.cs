
namespace Baseball.Infrastructure.Repository
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Task AddAsync(T entity);

        Task<T> GetByIdAsync(object id);

        IQueryable<T> GetAll();

        void UpdateAsync(T entity);

        Task<int> SaveChangesAsync();
    }
}
