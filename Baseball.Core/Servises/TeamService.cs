using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class TeamService : ITeamService
    {
        private readonly IRepository repository;

        public TeamService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(AddTeamModel model)
        {
            var team = new Team()
            {
                Name = model.Name,
                HomeColor = model.HomeColor,
                AwayColor = model.AwayColor,
            };

            await repository.AddAsync(team);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TeamViewModel>> GetAllAsync()
        {
            var teams = await repository.GetAll<Team>()
                .Include(x => x.AwayGames)
                .Include(x => x.HomeGames)
                .Include(x => x.Players)
                .Include(x => x.ChampionShips)
                .Where(t => t.IsDeleted == false)
                .Select(t => new TeamViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            foreach (var team in teams)
            {
                team.WinGames = await GetWinsAsync(await GetEntityByIdAsync(team.Id));
                team.LoseGames = await GetLosesAsync(await GetEntityByIdAsync(team.Id));
            }

            teams
              .OrderByDescending(t => t.WinGames)
              .ThenBy(t => t.LoseGames);

            return teams;
        }

        public async Task<EditTeamViewModel?> GetByIdAsync(int id)
        {
            return await GetById(id)
                .Select(t => new EditTeamViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    HomeColor = t.HomeColor,
                    AwayColor = t.AwayColor
                })
                .SingleOrDefaultAsync();
        }

        private IQueryable<Team> GetById(int id)
        {
            return repository.GetAll<Team>()
                .Where(t => t.Id == id);
        }

        public async Task<TeamDetailsViewModel> GetDetailsAsync(int id)
        {
            var team = await GetById(id)
                .Select(t => new TeamDetailsViewModel()
                {
                    Name = t.Name,
                    HomeColor = t.HomeColor,
                    AwayColor = t.AwayColor,
                    WinGames = t.WinGames,
                    loseGames = t.loseGames,
                    Players = t.Players
                    .Select(p => new PlayerViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Number = p.Number,
                        Position = p.Position,
                        Bat = $"{p.Bat.Brand} - {p.Bat.BatMaterial.Name} {p.Bat.Size}inch.",
                        Glove = p.Glove,
                        ThrowHand = p.ThrowHand,
                        BattingAverage = p.BattingAverage.ToString("F3")
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if (team == null)
            {
                return null;
            }

            team.WinGames = await GetWinsAsync(await GetEntityByIdAsync(id));
            team.loseGames = await GetLosesAsync(await GetEntityByIdAsync(id));

            return team;
        }

        public async Task UpdateAsync(int id, EditTeamViewModel model)
        {
            var team = await GetEntityByIdAsync(id);

            if (team == null)
            {
                throw new ArgumentNullException($"Team with id {id} was not found.");
            }

            team!.Name = model.Name;
            team.HomeColor = model.HomeColor;
            team.AwayColor = model.AwayColor;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var team = await GetEntityByIdAsync(id);

            if (team == null)
            {
                throw new ArgumentNullException($"Team with id {id} was not found.");
            }

            team!.IsDeleted = true;

            await repository.SaveChangesAsync();
        }

        public async Task<List<TeamNameViewModel>> GetAllTeamNamesAsync()
        {
            return await repository.GetAll<Team>()
                .Where(t => t.IsDeleted == false)
                .Select(t => new TeamNameViewModel()
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public async Task<Team> GetEntityByIdAsync(int id)
        {
            return await GetById(id)
                .Include(x => x.HomeGames)
                .Include(x => x.AwayGames)
                .Include(x => x.ChampionShips)
                .Include(x => x.Players)
                .FirstOrDefaultAsync();
        }

        private async Task<List<Game>> GetAllTeamGames(Team team)
        {
            return await repository.GetAll<Game>()
                .Include(g => g.Winner)
                .Where(g => g.IsDeleted == false && (g.HomeTeam == team || g.AwayTeam == team))
                .ToListAsync();
        }

        public async Task<int> GetWinsAsync(Team team)
        {
            var games = await GetAllTeamGames(team);

            return games
                .Where(g => g.Winner == team)
                .Count();
        }

        public async Task<int> GetLosesAsync(Team team)
        {
            var games = await GetAllTeamGames(team);

            return games
                .Where(g => g.Winner != team && g.Winner != null)
                .Count();
        }
    }
}
