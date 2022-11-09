using Baseball.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private BaseballDbContext context;
        private DbSet<T> entities;

        public Repository(BaseballDbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await entities.AddAsync(entity);
        }

        public void Dispose()
        {
            context.DisposeAsync();
        }

        public IQueryable<T> GetAll()
        {
            return entities.AsQueryable();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            T? entity = await entities.FindAsync(id);

            return entity;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void UpdateAsync(T entity)
        {
            context.Update(entity);
        }
    }
}
