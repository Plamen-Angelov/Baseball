using Baseball.Infrastructure.Data;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.UnitTests.RepositoryTests
{
    public class RepositoryTests
    {
        private IEnumerable<Bat> bats;
        private BaseballDbContext context;
        private IRepository repository;

        [SetUp]
        public void Setup()
        {
            bats = new List<Bat>()
            {
                new Bat() { Id = 1, Brand = "Mizuno", BatMaterialId = 1, Size = 33, IsDeleted = false },
                new Bat() { Id = 2,  Brand = "Mizuno", BatMaterialId = 1, Size = 32, IsDeleted = true },
                new Bat() { Id = 3, Brand = "E7", BatMaterialId = 1, Size = 34, IsDeleted = false }
            };

            var options = new DbContextOptionsBuilder<BaseballDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .Options;

            context = new BaseballDbContext(options);
            context.AddRange(bats);
            context.SaveChanges();

            repository = new Repository(context);
        }

        [TearDown]
        public void CleanUp()
        {
            context.RemoveRange(bats);
            context.SaveChanges(true);
        }

        [Test]
        public void GetAll_ShouldReturnAll()
        {
            var result = repository.GetAll<Bat>().ToList();

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void Add_ShouldAddBatSuccessfully()
        {
            var bat = new Bat() { Brand = "Mizuno", BatMaterialId = 2, Size = 28 };

            repository.AddAsync(bat);
            repository.SaveChangesAsync();
            var result = repository.GetAll<Bat>().ToList();

            Assert.IsTrue(result.Count() == 4);
            Assert.IsTrue(result.Any(r => r.Brand == "Mizuno" && r.BatMaterialId == 2 && r.Size == 28));

            context.Remove(bat);
            context.SaveChangesAsync();
        }

        [Test]
        public void Update_ShouldWorkProperly()
        {
            var bat = repository.GetAll<Bat>().First(x => x.Id == 1);

            bat.Brand = "Demarini";
            bat.Size = 30;
            repository.UpdateAsync(bat);
            repository.SaveChangesAsync();

            var result = repository.GetAll<Bat>().First(x => x.Id == 1);

            Assert.IsTrue(result.Brand == "Demarini");
            Assert.IsTrue(result.Size == 30);
        }
    }
}
