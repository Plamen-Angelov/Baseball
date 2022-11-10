using Baseball.Infrastructure.Data;
using Baseball.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Baseball.Infrastructure.Repository
{
    public class BatRepository : IRepository<Bat>
    {
        private readonly BaseballDbContext context;

        public BatRepository(BaseballDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Bat entity)
        {
            await context.AddAsync(entity);
        }

        public async Task<List<Bat>> GetAllAsync()
        {
            return await context.Bats
                .Include(b => b.BatMaterial)
                .Where(b => b.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Bat?> GetByIdAsync(object id)
        {
            return await context.Bats
                .Include(b => b.BatMaterial)
                .Where(b => b.IsDeleted == false)
                .FirstOrDefaultAsync(b => b.Id == (int)id);
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public void UpdateAsync(Bat entity)
        {
            context.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            //context.Update(entity);
        }

        public void Dispose()
        {
            context.DisposeAsync();
        }
    }
}
