using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Core.Contracts;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Moq;

namespace Baseball.UnitTests.ServicesTests
{
    public class GameServiceTests
    {
        private InMemoryDatabase inMemoryDb;
        private IRepository repository;
        private IChampionShipService championShipService;
        private ITeamService teamService;
        private GameService gameService;

        [SetUp]
        public async Task Setup()
        {
            inMemoryDb = new InMemoryDatabase();
            repository = new Repository(inMemoryDb.Context);
            teamService = new TeamService(repository);
            championShipService = new ChampionShipService(repository, teamService);
            gameService = new GameService(repository, championShipService, teamService);

            var games = GetGames();
            inMemoryDb.Context.AddRange(games);

            var championShip = new ChampionShip()
            {
                Name = "National ChampionScip",
                Year = 2022
            };
            inMemoryDb.Context.Add(championShip);

            var teams = new List<Team>()
            {
                new Team() { Name = "Team Varna", HomeColor = "Blue", AwayColor = "Gray" },
                new Team() { Name = "Team Sofia", HomeColor = "Red", AwayColor = "White" }
            };
            await inMemoryDb.Context.AddRangeAsync(teams);
            await repository.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            inMemoryDb.Context.Dispose();
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(3, 7)]
        [TestCase(6, 4)]
        public async Task Add_ShouldAddGame_WhenPasValidModel(int awRuns, int htRuns)
        {
            var game = new AddGameViewModel()
            {
                ChampionShipId = 1,
                AwayTeamId = 2,
                HomeTeamId = 1,
                Stadium = "Octomvri",
                AwayTeamRuns = awRuns,
                HomeTeamRuns = htRuns
            };

            await gameService.AddAsync(game);
            var games = await gameService.GetAllAsync();

            Assert.IsTrue(games.Count() == 3);
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var all = await gameService.GetAllAsync();

            Assert.IsFalse(all == default);
            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().GetType() == typeof(GameViewModel));
        }

        [Test]
        public async Task GetById_ShouldReturnGameSuccessfully()
        {
            var game = await gameService.GetByIdAsync(2);

            Assert.IsNotNull(game);
            Assert.IsTrue(game.Id == 2);
            Assert.IsTrue(game.GetType() == typeof(EditGameViewModel));
        }

        [Test]
        [TestCase(3)]
        [TestCase(6)]
        public async Task GetById_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var game = await gameService.GetByIdAsync(id);

            Assert.IsNull(game);
        }

        [Test]
        public async Task Update_ShouldUpgateGame_WhenPassCorrectData()
        {
            var model = new EditGameViewModel()
            {
                ChampionShipId = 1,
                AwayTeamId = 2,
                HomeTeamId = 1,
                Stadium = "Octomvri"
            };

            await gameService.UpdateAsync(1, model);
            var game = await gameService.GetByIdAsync(1);

            Assert.IsTrue(game!.Stadium == "Octomvri");
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Updat_ShouldThrowException_WhenGameisNotFound(int id)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await gameService.UpdateAsync(id, new EditGameViewModel()));
        }

        [Test]
        public async Task Delete_ShouldDeleteGame_WhenPassCorrecrId()
        {
            await gameService.DeleteAsync(1);
            await gameService.DeleteAsync(2);

            var games = await gameService.GetAllAsync();

            Assert.IsTrue(games.Count() == 0);
            Assert.IsFalse(games.Any(x => x.Id == 1));
        }

        [Test]
        [TestCase(3)]
        [TestCase(4)]
        public async Task Delete_ShouldThrowArgumentNullException_WhenPassNotCorrecrId(int id)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await gameService.DeleteAsync(id));
        }

        private List<Game> GetGames()
        {
            return new List<Game>()
            {
                new Game()
                {
                    Id = 1,
                    ChampionShipId = 1,
                    AwayTeamId = 2,
                    HomeTeamId = 1,
                    Stadium = "Diana",
                    IsDeleted = false
                },
                new Game()
                {
                    Id = 2,
                    ChampionShipId = 1,
                    AwayTeamId = 1,
                    HomeTeamId = 2,
                    Stadium = "Diana",
                    IsDeleted = false
                },
                new Game()
                {
                    Id = 3,
                    ChampionShipId = 1,
                    AwayTeamId = 2,
                    HomeTeamId = 3,
                    Stadium = "Diana",
                    IsDeleted = true
                }
            };
        }
    }
}
