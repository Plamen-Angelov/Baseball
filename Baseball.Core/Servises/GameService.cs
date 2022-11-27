using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;

        public GameService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task AddAsync(AddGameViewModel model)
        {
            var game = new Game()
            {
                ChampionShipId = model.ChampionShipId,
                AwayTeamId = model.AwayTeamId,
                HomeTeamId = model.HomeTeamId,
                Stadium = model.Stadium,
                InningPlayed = model.InningPlayed,
                AwayTeamRuns = model.AwayTeamRuns,
                HomeTeamRuns = model.HomeTeamRuns,
                AwayTeamHits = model.AwayTeamHits,
                HomeTeamHits = model.HomeTeamHits,
                AwayTeamErrors = model.AwayTeamErrors,
                HomeTeamErrors = model.HomeTeamErrors
            };

            await repository.AddAsync(game);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GameViewModel>> GetAllAsync()
        {
            return await repository.GetAll<Game>()
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
                })
                .ToListAsync();
        }

        public async Task<EditGameViewModel> GetByIdAsync(int id)
        {
            return await GetById(id)
                .Select(g => new EditGameViewModel()
                {
                    Id = g.Id,
                    ChampionShipId = g.ChampionShipId,
                    AwayTeamId = g.AwayTeamId,
                    HomeTeamId = g.HomeTeamId,
                    Stadium = g.Stadium,
                    InningPlayed = g.InningPlayed,
                    AwayTeamRuns = g.AwayTeamRuns,
                    HomeTeamRuns = g.HomeTeamRuns,
                    AwayTeamHits = g.AwayTeamHits,
                    HomeTeamHits = g.AwayTeamHits,
                    AwayTeamErrors = g.AwayTeamErrors,
                    HomeTeamErrors = g.AwayTeamErrors
                })
                .SingleAsync();
        }

        private IQueryable<Game> GetById(int id)
        {
            return repository.GetAll<Game>()
                .Where(g => g.IsDeleted == false && g.Id == id);
        }

        public async Task UpdateAsync(int id, EditGameViewModel model)
        {
            var game = await GetById(id)
                .FirstOrDefaultAsync();

            game.ChampionShipId = model.ChampionShipId;
            game.AwayTeamId = model.AwayTeamId;
            game.HomeTeamId = model.HomeTeamId;
            game.Stadium = model.Stadium;
            game.InningPlayed = model.InningPlayed;
            game.AwayTeamRuns = model.AwayTeamRuns;
            game.HomeTeamRuns = model.HomeTeamRuns;
            game.AwayTeamHits = model.AwayTeamHits;
            game.HomeTeamHits = game.HomeTeamHits;
            game.AwayTeamErrors = model.AwayTeamErrors;
            game.HomeTeamErrors = game.AwayTeamErrors;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var game = await GetById(id)
                .FirstOrDefaultAsync();

            game.IsDeleted = true;

            await repository.SaveChangesAsync();
        }
    }
}
