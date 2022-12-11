using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace Baseball.UnitTests.ServicesTests
{
    public class BatServiceTests
    {
        private BatMaterialService batMaterialService;
        private Mock<ILogger<BatService>> mockLogger;
        private BatService batService;
        private IRepository repository;
        private InMemoryDatabase inMemoryDb;

        [SetUp]
        public async Task Setup()
        {
            inMemoryDb = new InMemoryDatabase();
            repository = new Repository(inMemoryDb.Context);
            mockLogger = new Mock<ILogger<BatService>>();
            batMaterialService = new BatMaterialService(repository);
            batService = new BatService(repository, batMaterialService, mockLogger.Object);

            var bats = GetBats();
            await inMemoryDb.Context.AddRangeAsync(bats);
            await inMemoryDb.Context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            inMemoryDb.Context.Dispose();
        }

        [Test]
        public async Task Add_ShouldAddBat_WhenPassValidModel()
        {
            var bat = new AddBatViewModel
            {
                Brand = "E7",
                Size = 33,
                MaterialId = 1
            };

            await batService.AddAsync(bat);
            var allBats = await batService.GetAllAsync();

            Assert.IsFalse(allBats == default);
            Assert.IsTrue(allBats.Count() == 3);
            Assert.IsTrue(allBats.Any(x => x.Brand == "E7" && x.Size == 33));
        }

        [Test]
        public async Task Add_ShouldThrowException_WhenPassNull()
        {
            Assert.ThrowsAsync<NullReferenceException>(async Task () => await batService.AddAsync(null));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var all = await batService.GetAllAsync();

            Assert.IsFalse(all == default);
            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().GetType() == typeof(BatViewModel));
            Assert.IsNotNull(all.First().Material);
        }

        [Test]
        public async Task GetById_ShouldTerurnCorrectModel()
        {
            var result = await batService.GetByIdAsync(3);

            Assert.IsTrue(result.Id == 3);
            Assert.IsTrue(result.Brand == "E7");
        }

        [Test]
        [TestCase(2)]
        [TestCase(8)]
        public async Task GetById_ShouldThrowArgumentException_WhenBatWasNotFound(int id)
        {
            Assert.ThrowsAsync<ArgumentException>(async Task () => await batService.GetByIdAsync(id));
        }

        [Test]
        public async Task Update_ShouldWorkProperly_WhenPassValidId()
        {
            var bat = new AddBatViewModel
            {
                Brand = "E7",
                Size = 30,
                MaterialId = 1
            };

            int id = 1;

            await batService.UpdateAsync(id, bat);
            var updatedBat = await batService.GetByIdAsync(1);

            Assert.IsNotNull(updatedBat);
            Assert.IsTrue(updatedBat.Brand == "E7" && updatedBat.Size == 30);
        }

        [Test]
        [TestCase(2)]
        [TestCase(6)]
        public async Task Update_ShouldThrowArgumentException_WhenBatWasNotFound(int id)
        {
            var bat = new AddBatViewModel();

            Assert.ThrowsAsync<ArgumentException>(async Task () => await batService.UpdateAsync(id, bat));
        }

        [Test]
        public async Task Delete_ShouldDeleteBat_WhenPassCorrectId()
        {
            await batService.DeleteAsync(1);
            var allBats = await batService.GetAllAsync();

            Assert.IsNotNull(allBats);
            Assert.IsTrue(allBats.Count() == 1);
            Assert.IsFalse(allBats.Any(b => b.id == 1));
        }

        [Test]
        [TestCase(2)]
        [TestCase(6)]
        public async Task Delete_ShouldThrowArgumentException_WhenBatWasNotFound(int id)
        {
            Assert.ThrowsAsync<ArgumentException>(async Task () => await batService.DeleteAsync(id));
        }

        private IQueryable<Bat> GetBats()
        {
            return new List<Bat>()
            {
                new Bat()
                {
                    Id = 1,
                    Brand = "Mizuno",
                    BatMaterialId = 1,
                    Size = 33,
                    IsDeleted = false
                },
                new Bat()
                {
                    Id = 2,
                    Brand = "Mizuno",
                    BatMaterialId = 1,
                    Size = 32,
                    IsDeleted = true
                },
                new Bat()
                {
                    Id = 3,
                    Brand = "E7",
                    BatMaterialId = 1,
                    Size = 34,
                    IsDeleted = false
                }
            }.AsQueryable();
        }
    }
}
