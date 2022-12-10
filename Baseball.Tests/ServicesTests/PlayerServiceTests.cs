using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;

namespace Baseball.UnitTests.ServicesTests
{
    public class PlayerServiceTests
    {
        private IRepository repository;
        private InMemoryDatabase inMemoryDb;
        private TeamService teamService;
        private PlayerService playerService;

        [SetUp]
        public async Task Setup()
        {
            inMemoryDb = new InMemoryDatabase();
            repository = new Repository(inMemoryDb.Context);
            teamService = new TeamService(repository);
            playerService = new PlayerService(repository, teamService);

            var players = GetPlayers();
            inMemoryDb.Context.AddRange(players);
            await repository.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            inMemoryDb.Context.Dispose();
        }

        [Test]
        public async Task Add_ShouldAddPlayer_WhenPassValidModel()
        {
            var player = new AddPlayerViewModel
            {
                Name = "Pesho1",
                Position = "P, C",
                BatId = 1,
                Glove = "Mizuno",
                ThrowHand = "right",
                BattingAverage = 0.370
            };

            await playerService.AddAsync(player);
            var allPlayers = await playerService.GetAllAsync();

            Assert.IsTrue(allPlayers.Count() == 3);
            Assert.IsTrue(allPlayers.Any(c => c.Name == "Pesho1"));
            Assert.IsTrue(allPlayers.First().GetType() == typeof(PlayerViewModel));
        }

        [Test]
        public async Task Delete_ShouldDeletePlayer_WhenPassCorrectId()
        {
            await playerService.DeleteAsync(1);
            var players = await playerService.GetAllAsync();

            Assert.IsTrue(players.Count() == 1);
            Assert.IsFalse(players.Any(c => c.Id == 1));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task Delete_ShouldThrowArgumentException_WhenPassNotCorrectId(int id)
        {
            Assert.ThrowsAsync<ArgumentException>(async Task () => await playerService.DeleteAsync(id));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var players = await playerService.GetAllAsync();

            Assert.IsTrue(players.Count() == 2);
            Assert.IsFalse(players.Any(x => x.Name == "Tosho"));
            Assert.IsTrue(players.First().GetType() == typeof(PlayerViewModel));
        }

        [Test]
        public async Task GetById_ShouldReturnPlayerSuccessfully()
        {
            var player = await playerService.GetByIdAsync(1);

            Assert.IsTrue(player.Id == 1);
            Assert.IsTrue(player.Name == "Pesho");
            Assert.IsTrue(player.GetType() == typeof(AddPlayerViewModel));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetById_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var player = await playerService.GetByIdAsync(id);

            Assert.IsNull(player);
        }

        [Test]
        public async Task Update_ShouldUpdatePlayer_WhenPaddCorrectdata()
        {
            var playerModel = new AddPlayerViewModel()
            {
                Name = "Pesho12",
                Position = "P",
                BatId = 1,
                Glove = "Academa",
                ThrowHand = "right",
                BattingAverage = 0.300,
            };

            await playerService.UpdateAsync(1, playerModel);
            var updated = await playerService.GetByIdAsync(1);

            Assert.IsTrue(updated!.Name == "Pesho12");
            Assert.IsTrue(updated.BattingAverage == 0.300);
        }

        [Test]
        [TestCase(3)]
        [TestCase(6)]
        public async Task Update_ShouldThrowArgumentException_WhenPassNotCorrectId(int id)
        {
            Assert.ThrowsAsync<ArgumentException>(async Task () => await playerService.UpdateAsync(id, new AddPlayerViewModel()));
        }

        [Test]
        public async Task GetPlayerById_ShouldPlayerSuccessfully()
        {
            var player = await playerService.GetPlayerByIdAsync(2);

            Assert.IsTrue(player.Id == 2);
            Assert.IsTrue(player.Name == "Gosho");
            Assert.IsTrue(player.GetType() == typeof(PlayerViewModel));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetPlayerById_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var player = await playerService.GetPlayerByIdAsync(id);

            Assert.IsNull(player);
        }

        [Test]
        public async Task AddToTeam_ShouldAddPlayerSuccessfully()
        {
            var teamToAdd = new Team()
            {
                Name = "Team Varna",
                HomeColor = "Blue",
                AwayColor = "Gray"
            };
            await inMemoryDb.Context.AddAsync(teamToAdd);
            await inMemoryDb.Context.SaveChangesAsync();

            await playerService.AddToTeamAsync(2, 1);

            var team = await teamService.GetEntityByIdAsync(1);

            Assert.IsNotNull(team.Players);
            Assert.IsTrue(team.Players.Count() == 1);
            Assert.IsTrue(team.Players.First().Id == 2);
        }

        [Test]
        [TestCase(3, 1)]
        [TestCase(6, 1)]
        [TestCase(2, 3)]
        public async Task AddToteam_ShouldThrowNullReferenceException_WhenPassNotCorrectId(int playerId, int teamId)
        {
            var teamToAdd = new Team()
            {
                Name = "Team Varna",
                HomeColor = "Blue",
                AwayColor = "Gray"
            };
            await inMemoryDb.Context.AddAsync(teamToAdd);
            await inMemoryDb.Context.SaveChangesAsync();

            Assert.ThrowsAsync<NullReferenceException>(async Task () => await playerService.AddToTeamAsync(playerId, teamId));
        }

        [Test]
        public async Task MakePlayerFreeAgent_ShouldRemovePlayerFromTeam()
        {
            var teamToAdd = new Team()
            {
                Name = "Team Varna",
                HomeColor = "Blue",
                AwayColor = "Gray"
            };
            await inMemoryDb.Context.AddAsync(teamToAdd);
            await inMemoryDb.Context.SaveChangesAsync();

            await playerService.AddToTeamAsync(2, 1);
            var team = await teamService.GetEntityByIdAsync(1);
            Assert.IsTrue(team.Players.Count() == 1);
            Assert.IsTrue(team.Players.First().Id == 2);

            await playerService.MakePlayerFreeAgentAsync(2);

            Assert.IsFalse(team.Players.Any());
        }

        [Test]
        [TestCase(3)]
        [TestCase(6)]
        public async Task MakePlayerFreeAgent_ShouldThrowNullReferenceException_WhenPassNotCorrectIdForPlayer(int playerId)
        {
            var teamToAdd = new Team()
            {
                Name = "Team Varna",
                HomeColor = "Blue",
                AwayColor = "Gray"
            };
            await inMemoryDb.Context.AddAsync(teamToAdd);
            await inMemoryDb.Context.SaveChangesAsync();

            await playerService.AddToTeamAsync(2, 1);
            var team = await teamService.GetEntityByIdAsync(1);
            Assert.IsTrue(team.Players.Count() == 1);
            Assert.IsTrue(team.Players.First().Id == 2);

            Assert.ThrowsAsync<NullReferenceException>(async Task () => await playerService.MakePlayerFreeAgentAsync(playerId));
        }

        [Test]
        public async Task MakePlayerFreeAgent_ShouldThrowNullReferenceException_WhenPlayersTeamIsNull()
        {
            Assert.ThrowsAsync<NullReferenceException>(async Task () => await playerService.MakePlayerFreeAgentAsync(2));
        }

        private List<Player> GetPlayers()
        {
            return new List<Player>()
            {
                new Player()
                {
                    Name = "Pesho",
                    Position = "P",
                    BatId = 1,
                    Bat = new Bat(){ Brand = "", BatMaterial = new BatMaterial() { Name = "WOOD"}, Size = 33},
                    Glove = "Academa",
                    ThrowHand = "right",
                    BattingAverage = 0.250,
                    IsDeleted = false
                },
                new Player()
                {
                    Name = "Gosho",
                    Position = "C,1B",
                    BatId = 2,
                    Bat = new Bat(){ Brand = "", BatMaterial = new BatMaterial() { Name = "WOOD"}, Size = 33},
                    Glove = "Academa",
                    ThrowHand = "left",
                    BattingAverage = 0.330,
                    IsDeleted = false
                },
                new Player()
                {
                    Name = "Tosho",
                    Position = "CF",
                    BatId = 1,
                    Bat = new Bat(){ Brand = "", BatMaterial = new BatMaterial() { Name = "WOOD" }, Size = 33},
                    Glove = "Academa",
                    ThrowHand = "right",
                    BattingAverage = 0.250,
                    IsDeleted = true
                }
            };
        }
    }
}
