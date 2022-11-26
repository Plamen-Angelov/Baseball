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
                    ChampionShipName = g.ChampionShip.Name,
                    AwayTeamName = g.AwayTeam.Name,
                    Stadium = g.HomeTeam.Name,
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
    }
}
