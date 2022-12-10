using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Repository;

namespace Baseball.UnitTests.ServicesTests
{
    public class BatMaterialServiceTests
    {
        private InMemoryDatabase inMemoryDatabase;
        private IRepository repository;
        private BatMaterialService batMaterialService;

        [SetUp]
        public void Setup()
        {
            inMemoryDatabase = new InMemoryDatabase();
            repository = new Repository(inMemoryDatabase.Context);
            batMaterialService = new BatMaterialService(repository);
        }

        [TearDown]
        public void TearDown()
        {
            inMemoryDatabase.Context.Dispose();
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var all = await batMaterialService.GetAllBatMaterialsAsync();

            Assert.IsNotNull(all);
            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().GetType() == typeof(BatMaterialViewModel));
        }
    }
}
