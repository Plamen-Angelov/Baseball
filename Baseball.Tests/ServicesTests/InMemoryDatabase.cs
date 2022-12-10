using Baseball.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Baseball.UnitTests.ServicesTests
{
    internal class InMemoryDatabase
    {
        private readonly BaseballDbContext context;
        public InMemoryDatabase()
        {
            var contextOptions = new DbContextOptionsBuilder<BaseballDbContext>()
                .UseInMemoryDatabase("AppInMemoryDb")
                .Options;
            context = new BaseballDbContext(contextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public BaseballDbContext Context => context;
    }
}
