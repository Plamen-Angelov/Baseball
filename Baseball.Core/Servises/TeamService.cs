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
                .Where(t => t.IsDeleted == false)
                .Select(b => new TeamViewModel()
                {
                    Id = b.Id,
                    Name = b.Name,
                    WinGames = b.WinGames,
                    LoseGames = b.loseGames
                })
                .OrderByDescending(t => t.WinGames)
                .ThenBy(t => t.LoseGames)
                .ToListAsync();

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

            return team;
        }

        public async Task UpdateAsync(int id, EditTeamViewModel model)
        {
            var team = await  GetById(id).FirstOrDefaultAsync();

            team!.Name = model.Name;
            team.HomeColor = model.HomeColor;
            team.AwayColor = model.AwayColor;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var team = await GetById(id).FirstOrDefaultAsync();

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
    }
}
