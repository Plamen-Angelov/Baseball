//using Baseball.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;

//namespace Baseball.Infrastructure.Repository
//{
//    public class Repository<DbModel> : IRepository<DbModel> where DbModel : class
//    {
//        private BaseballDbContext context;
//        private DbSet<DbModel> entities;

//        public Repository(BaseballDbContext context)
//        {
//            this.context = context;
//            entities = context.Set<DbModel>();
//        }

//        public async Task AddAsync(DbModel entity)
//        {
//            await entities.AddAsync(entity);
//        }

//        public void Dispose()
//        {
//            context.DisposeAsync();
//        }

//        //public IQueryable<DbModel> GetAllAsync()
//        //{
//        //    return entities.AsQueryable();
//        //}

//        public async Task<DbModel> GetByIdAsync(object id)
//        {
//            DbModel? entity = await entities.FindAsync(id);

//            return entity;
//        }

//        public async Task<int> SaveChangesAsync()
//        {
//            return await context.SaveChangesAsync();
//        }

//        public void UpdateAsync(DbModel entity)
//        {
//            context.Update(entity);
//        }

//        public async Task<List<DbModel>> GetAllAsync()
//        {
//            return await entities.ToListAsync();
//        }
//    }
//}
