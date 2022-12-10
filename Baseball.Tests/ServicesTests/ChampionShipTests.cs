using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;

namespace Baseball.UnitTests.ServicesTests
{
    public class ChampionShipTests
    {
        private InMemoryDatabase inmemoryDb;
        private Repository repository;
        private ChampionShipService championShipService;
        private TeamService teamService;

        [SetUp]
        public async Task Setup()
        {
            inmemoryDb = new InMemoryDatabase();
            repository = new Repository(inmemoryDb.Context);
            teamService = new TeamService(repository);
            championShipService = new ChampionShipService(repository, teamService);

            var championShips = GetChampionShips();
            foreach (var championShip in championShips)
            {
                await repository.AddAsync(championShip);
            }

            await repository.SaveChangesAsync();
        }


        [TearDown]
        public void TearDown()
        {
            inmemoryDb.Context.Dispose();
        }

        [Test]
        public async Task Add_ShouldAddChampionShip_WhenAddValidChamponShip()
        {
            var championShip = new AddChampionShipViewModel()
            {
                Name = "Bulgaria Cup",
                Year = 2022
            };

            await championShipService.AddAsync(championShip);
            var all = await championShipService.GetAllAsync();

            Assert.IsTrue(all.Count() == 3);
            Assert.IsTrue(all.Any(c => c.Name == "Bulgaria Cup"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var all = await championShipService.GetAllAsync();

            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().Year == 2022);
            Assert.IsTrue(all.First().GetType() == typeof(ChampionShipViewModel));
        }

        [Test]
        public async Task GetDetails_ShouldReturnDetailsSuccessfully()
        {
            var teamDetails = await championShipService.GetDetailsAsync(2);

            Assert.IsNotNull(teamDetails);
            Assert.IsTrue(teamDetails.GetType() == typeof(ChampionShipDetailsViewModel));
            Assert.IsTrue(teamDetails.Name == "National championShip - 2022");
            Assert.IsNotNull(teamDetails.Games);
            Assert.IsTrue(teamDetails.Games.GetType() == typeof(List<GameViewModel>));
            Assert.IsNotNull(teamDetails.Teams);
            Assert.IsTrue(teamDetails.Teams.GetType() == typeof(List<TeamViewModel>));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetDetails_ShouldThrowArgumentException_WhenChampionShipIsNotFound(int id)
        {
            Assert.ThrowsAsync<InvalidOperationException>(async Task () => await championShipService.GetDetailsAsync(id));
        }

        [Test]
        public async Task GetAllChampionShipNames_ShouldReturnAllSuccessfully()
        {
            var championShips = await championShipService.GetAllChampionShipNamesAsync();

            Assert.IsTrue(championShips.Count() == 2);
            Assert.IsTrue(championShips.First().GetType() == typeof(ChampionShipNameViewModel));
        }

        [Test]
        public async Task GetById_ShouldReturnChampionShip_WhenPassCorrectId()
        {
            var championShip = await championShipService.GetByIdAsync(1);

            Assert.IsNotNull(championShip);
            Assert.IsTrue(championShip.Name == "National championShip" && championShip.Year == 2020);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public async Task GetById_ShouldReturnNull_WhenChampionShipIsNotFound(int id)
        {
            var championShip = await championShipService.GetByIdAsync(id);

            Assert.IsNull(championShip);
        }

        [Test]
        public async Task Update_ShouldUpdateChampionShip()
        {
            var updatedInput = new EditChampionShipViewModel()
            {
                Name = "National",
                Year = 2021,
            };

            await championShipService.UpdateAsync(1, updatedInput);
            var updated = await championShipService.GetByIdAsync(1);

            Assert.IsTrue(updated!.Name == "National");
            Assert.IsTrue(updated.Year == 2021);
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Update_ShouldThrowArgumentNullException_WhenPassedIdIsNotCorrect(int id)
        {
            var updatedInput = new EditChampionShipViewModel();

            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await championShipService.UpdateAsync(id, updatedInput));
        }

        [Test]
        public async Task Delete_ShouldDeleteChampionShip()
        {
            await championShipService.DeleteAsync(1);

            var all = await championShipService.GetAllAsync();

            Assert.That(all.Count() == 1);
            Assert.IsFalse(all.Any(c => c.Year == 2020));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Delete_ShouldThrowArgumentNullException_WhenPassedIdIsNotCorrect(int id)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await championShipService.DeleteAsync(id));
        }

        [Test]
        public async Task GetHomePageAll_ShouldReturnAllSuccessfully()
        {
            var all = await championShipService.GetHomePageAllAsync();

            Assert.IsTrue(all.Count() == 1);
            Assert.IsTrue(all.First().ChampionShipYear == 2022 && all.First().ChampionShipName == "National championShip");
            Assert.IsTrue(all.First().GetType() == typeof(HomePageViewModel));
            Assert.IsNotNull(all.First().Teams);
        }

        private List<ChampionShip> GetChampionShips()
        {
            return new List<ChampionShip>()
            {
                new ChampionShip()
                {
                    Id = 1,
                    Name = "National championShip",
                    Year = 2020,
                    IsDeleted = false
                },
                new ChampionShip()
                {
                    Id = 2,
                    Name = "National championShip",
                    Year = 2022,
                    IsDeleted = false
                },
                new ChampionShip()
                {
                    Id = 3,
                    Name = "Sofia Cup",
                    Year = 2022,
                    IsDeleted = true
                }
            };
        }
    }
}
