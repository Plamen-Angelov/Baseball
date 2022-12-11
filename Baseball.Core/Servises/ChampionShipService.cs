using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class ChampionShipService : IChampionShipService
    {
        private readonly IRepository repository;
        private readonly ITeamService teamService;

        public ChampionShipService(IRepository repository, ITeamService teamService)
        {
            this.repository = repository;
            this.teamService = teamService;
        }

        public async Task AddAsync(AddChampionShipViewModel model)
        {
            var championShip = new ChampionShip()
            {
                Name = model.Name,
                Year = model.Year
            };

            await repository.AddAsync(championShip);
            await repository.SaveChangesAsync();
        }

        public async Task<List<ChampionShipViewModel>> GetAllAsync()
        {
            return await repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false)
                .Select(c => new ChampionShipViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Year = c.Year
                })
                .OrderByDescending(c => c.Year)
                .ToListAsync();
        }

        public async Task<ChampionShipDetailsViewModel> GetDetailsAsync(int id)
        {
            var championShipDetails = await GetById(id)
                .Select(c => new ChampionShipDetailsViewModel()
                {
                    Id = c.Id,
                    Name = $"{c.Name} - {c.Year}",
                    Games = c.Games
                    .Where(g => g.IsDeleted == false)
                    .Select(g => new GameViewModel()
                    {
                        Id = g.Id,
                        ChampionShip = $"{g.ChampionShip.Name} - {g.ChampionShip.Year}",
                        AwayTeamName = g.AwayTeam.Name,
                        HomeTeamName = g.HomeTeam.Name,
                        Stadium = g.Stadium,
                        InningPlayed = g.InningPlayed,
                        AwayTeamRuns = g.AwayTeamRuns,
                        HomeTeamRuns = g.HomeTeamRuns,
                        AwayTeamHits = g.AwayTeamHits,
                        HomeTeamHits = g.AwayTeamHits,
                        AwayTeamErrors = g.AwayTeamErrors,
                        HomeTeamErrors = g.AwayTeamErrors
                    }).ToList(),
                    Teams = c.Teams
                    .Where(t => t.IsDeleted == false)
                    .Select(t => new TeamViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .ToList()
                })
                .FirstOrDefaultAsync();

            if (championShipDetails == null)
            {
                throw new ArgumentNullException("Championship was not found");
            }

            foreach (var team in championShipDetails.Teams)
            {
                var teamEntity = await teamService.GetEntityByIdAsync(team.Id);
                team.WinGames = await teamService.GetWinsAsync(teamEntity);
                team.LoseGames = await teamService.GetLosesAsync(teamEntity);
            }

            championShipDetails.Teams = championShipDetails.Teams
                .OrderByDescending(t => t.WinGames)
                .ThenBy(t => t.LoseGames)
                .ToList();

            return championShipDetails;
        }

        public async Task<List<ChampionShipNameViewModel>> GetAllChampionShipNamesAsync()
        {
            return await repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false)
                .Select(c => new ChampionShipNameViewModel()
                {
                    Id = c.Id,
                    Name = $"{c.Name} - {c.Year}"
                })
                .ToListAsync();
        }

        public Task<EditChampionShipViewModel?> GetByIdAsync(int id)
        {
            return GetById(id)
                .Select(c => new EditChampionShipViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Year = c.Year
                })
                .FirstOrDefaultAsync();
        }

        private IQueryable<ChampionShip> GetById(int id)
        {
            return repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false && c.Id == id);
        }

        public async Task UpdateAsync(int id, EditChampionShipViewModel model)
        {
            var championShip = await GetById(id).FirstOrDefaultAsync();

            if (championShip == null)
            {
                throw new ArgumentNullException($"Championship with id {id} was not found");
            }

            championShip.Name = model.Name;
            championShip.Year = model.Year;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var championShip = await GetEntityByIdAsync(id);

            if (championShip == null)
            {
                throw new ArgumentNullException($"Championship with id {id} was not found");
            }

            championShip.IsDeleted = true;

            await repository.SaveChangesAsync();
        }

        public async Task<ChampionShip?> GetEntityByIdAsync(int id)
        {
            return await GetById(id)
                .Include(c => c.Teams)
                .Include(c => c.Games)
                .FirstOrDefaultAsync();
        }

        public async Task<List<HomePageViewModel>> GetHomePageAllAsync()
        {
            var championShips = await repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false && c.Year == DateTime.Now.Year)
                .Select(c => new HomePageViewModel()
                {
                    ChampionShipName = c.Name,
                    ChampionShipYear = c.Year,
                    Teams = c.Teams
                    .Where(t => t.IsDeleted == false)
                    .Select(t => new TeamScoreViewModel()
                    {
                        Id = t.Id,
                        Name = t.Name
                    })
                    .ToList()
                })
                .ToListAsync();

            foreach (var championShip in championShips)
            {
                foreach (var team in championShip.Teams)
                {
                    var teamEntity = await teamService.GetEntityByIdAsync(team.Id);
                    team.WinGames = await teamService.GetWinsAsync(teamEntity);
                    team.LoseGames = await teamService.GetLosesAsync(teamEntity);
                }

                championShip.Teams = championShip.Teams
                                .OrderByDescending(t => t.WinGames)
                                .ThenBy(t => t.LoseGames)
                                .ToList();
            }

            return championShips;
        }
    }
}
