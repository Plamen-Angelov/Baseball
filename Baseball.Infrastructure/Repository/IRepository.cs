
namespace Baseball.Infrastructure.Repository
{
    internal interface IRepository<T> : IDisposable where T : class
    {
        Task AddAsync(T entity);

        Task<T> GetByIdAsync(object id);

        IEnumerable<T> GetAllAsync();

        void UpdateAsync(T entity);

        Task<int> SaveChangesAsync();
    }
}
