using Baseball.Infrastructure.Data;
using Baseball.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Infrastructure.Repository
{
    public class Repository : IRepository
    {
        private BaseballDbContext context;

        private DbSet<T> DbSet<T>() where T : class
        {
            return context.Set<T>();
        }

        public Repository(BaseballDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.AddAsync(entity);
        }

        public void Dispose()
        {
            context.DisposeAsync();
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return DbSet<T>()
                .AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void UpdateAsync<T>(T entity) where T : class
        {
            DbSet<T>().Update(entity);
        }
    }
}
