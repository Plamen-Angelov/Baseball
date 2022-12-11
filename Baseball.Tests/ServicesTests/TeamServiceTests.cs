using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Contracts;
using Baseball.Core.Servises;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;

namespace Baseball.UnitTests.ServicesTests
{
    public class TeamServiceTests
    {
        private InMemoryDatabase inMemoryDb;
        private IRepository repository;
        private ITeamService teamService;

        [SetUp]
        public async Task Setup()
        {
            inMemoryDb = new InMemoryDatabase();
            repository = new Repository(inMemoryDb.Context);
            teamService = new TeamService(repository);

            var teams = GetTeams();
            await inMemoryDb.Context.AddRangeAsync(teams);
            await inMemoryDb.Context.SaveChangesAsync();
        }

        [Test]
        public async Task Add_ShouldAddTeamSuccessfully()
        {
            var team = new AddTeamModel() { Name = "Junak", HomeColor = "Red", AwayColor = "Gray" };

            await teamService.AddAsync(team);
            var all = await teamService.GetAllAsync();

            Assert.IsTrue(all.Count() == 3);
            Assert.IsTrue(all.Any(x => x.Name == "Junak"));
        }

        [Test]
        public async Task GetAll_ShouldReturnAllSuccessfully()
        {
            var all = await teamService.GetAllAsync();

            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().GetType() == typeof(TeamViewModel));
        }

        [Test]
        public async Task GetById_ShouldReturnTeam_WhenPassCorrectId()
        {
            var team = await teamService.GetByIdAsync(1);

            Assert.IsNotNull(team);
            Assert.IsTrue(team.Name == "Buffaloes");
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetById_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var team = await teamService.GetByIdAsync(id);

            Assert.IsNull(team);
        }

        [Test]
        public async Task GetDetails_ShouldReturnDetails_WhenPassCorrectId()
        {
            var team = await teamService.GetDetailsAsync(2);

            Assert.IsNotNull(team);
            Assert.IsTrue(team.Name == "Athletic's");
            Assert.IsNotNull(team.Players);
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetDetails_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var team = await teamService.GetDetailsAsync(id);

            Assert.IsNull(team);
        }

        [Test]
        public async Task Update_Shouldupdateteam_WhenPassCorrectData()
        {
            var teamToUpdate = new EditTeamViewModel()
            {
                Name = "Name Changed",
                HomeColor = "dark Blue",
                AwayColor = "black/White",
            };

            await teamService.UpdateAsync(1, teamToUpdate);
            var updated = await teamService.GetByIdAsync(1);

            Assert.IsTrue(updated.Id == 1);
            Assert.IsTrue(updated.Name == "Name Changed");
        }

        [Test]
        [TestCase(3)]
        [TestCase(8)]
        public async Task Update_ShouldThrowArgumentNullException_WhenPassNotCorrectData(int id)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await teamService.UpdateAsync(id, new EditTeamViewModel()));
        }

        [Test]
        public async Task Delete_ShoulddeleteTeam_WhenPasssCorrectId()
        {
            await teamService.DeleteAsync(1);

            var all = await teamService.GetAllAsync();

            Assert.IsTrue(all.Count() == 1);
            Assert.IsFalse(all.Any(x => x.Id == 1));
        }

        [Test]
        [TestCase(3)]
        [TestCase(8)]
        public async Task Delete_ShouldThrowArgumentNullException_WhenPassNotCorrectId(int id)
        {
            Assert.ThrowsAsync<ArgumentNullException>(async Task () => await teamService.DeleteAsync(id));
        }

        [Test]
        public async Task GetAllTeamNames_ShouldReturnAllSuccessfully()
        {
            var all = await teamService.GetAllTeamNamesAsync();

            Assert.IsTrue(all.Count() == 2);
            Assert.IsTrue(all.First().GetType() == typeof(TeamNameViewModel));
        }

        [Test]
        public async Task GetEntityById_ShouldReturnTeam_WhenPassCorrectId()
        {
            var team = await teamService.GetEntityByIdAsync(1);

            Assert.IsNotNull(team);
            Assert.IsTrue(team.Name == "Buffaloes");
            Assert.IsTrue(team.GetType() == typeof(Team));
        }

        [Test]
        [TestCase(3)]
        [TestCase(5)]
        public async Task GetEntityById_ShouldReturnNull_WhenPassNotCorrectId(int id)
        {
            var team = await teamService.GetByIdAsync(id);

            Assert.IsNull(team);
        }

        [TearDown]
        public void TearDown()
        {
            inMemoryDb.Context.Dispose();
        }

        public List<Team> GetTeams()
        {
            return new List<Team>()
            {
                new Team()
                {
                    Name = "Buffaloes",
                    HomeColor = "dark Blue",
                    AwayColor = "black/White",
                    WinGames = 2,
                    loseGames = 1
                },
                new Team()
                {
                    Name = "Athletic's",
                    HomeColor = "Green",
                    AwayColor = "Yellow",
                    WinGames = 2
                },
                new Team()
                {
                    Name = "Lions",
                    HomeColor = "Blue",
                    AwayColor = "Yellow",
                    IsDeleted = true
                }
            };
        }
    }
}
