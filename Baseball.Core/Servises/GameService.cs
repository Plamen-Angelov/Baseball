﻿using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class GameService : IGameService
    {
        private readonly IRepository repository;
        private readonly IChampionShipService championShipService;
        private readonly ITeamService teamService;

        public GameService(IRepository repository,
            IChampionShipService championShipService,
            ITeamService teamService)
        {
            this.repository = repository;
            this.championShipService = championShipService;
            this.teamService = teamService;
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

            if (model.AwayTeamRuns > model.HomeTeamRuns)
            {
                game.WinnerId = model.AwayTeamId;
            }
            else if (model.AwayTeamRuns < model.HomeTeamRuns)
            {
                game.WinnerId = model.HomeTeamId; ;
            }

            await AddTeamsToChampionShipIfNotThere(model.AwayTeamId, model.HomeTeamId, model.ChampionShipId);
            SetWinner(game);
            await AddGameToTeams(game);

            await repository.AddAsync(game);
            await repository.SaveChangesAsync();
        }

        private void SetWinner(Game game)
        {
            if (game.HomeTeamRuns > game.AwayTeamRuns)
            {
                game.Winner = game.HomeTeam;
            }
            else if (game.HomeTeamRuns < game.AwayTeamRuns)
            {
                game.Winner = game.AwayTeam;
            }
        }

        private async Task AddTeamsToChampionShipIfNotThere(int awayTeamId, int homeTeamId, int championShipId)
        {
            var awayTeam = await teamService.GetEntityByIdAsync(awayTeamId);

            if (awayTeam == null)
            {
                throw new ArgumentNullException("Away team was not found.");
            }

            var homeTeam = await teamService.GetEntityByIdAsync(homeTeamId);

            if (homeTeam == null)
            {
                throw new ArgumentNullException("Home team was not found.");
            }

            var championShip = await championShipService.GetEntityByIdAsync(championShipId);

            if (championShip == null)
            {
                throw new ArgumentNullException("ChampionShip was not found.");
            }

            if (!championShip.Teams.Any(t => t.Name == awayTeam.Name))
            {
                championShip.Teams.Add(awayTeam);
            }

            if (!championShip.Teams.Any(t => t.Name == homeTeam.Name))
            {
                championShip.Teams.Add(homeTeam);
            }
        }

        private async Task AddGameToTeams(Game game)
        {
            var homeTeam = await teamService.GetEntityByIdAsync(game.HomeTeamId);
            homeTeam.HomeGames.Add(game);

            var awayTeam = await teamService.GetEntityByIdAsync(game.AwayTeamId);
            awayTeam.AwayGames.Add(game);
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
                    HomeTeamErrors = g.AwayTeamErrors,
                    WinnerId = g.WinnerId
                })
                .ToListAsync();
        }

        public async Task<EditGameViewModel?> GetByIdAsync(int id)
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
                .FirstOrDefaultAsync();
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

            if (game == null)
            {
                throw new ArgumentNullException($"Game with id {id} was not found.");
            }

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

            await AddTeamsToChampionShipIfNotThere(model.AwayTeamId, model.HomeTeamId, model.ChampionShipId);
            SetWinner(game);

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var game = await GetById(id)
                .Include(g => g.ChampionShip)
                .ThenInclude(c => c.Games)
                .ThenInclude(g => g.AwayTeam)
                .Include(g => g.ChampionShip)
                .ThenInclude(c => c.Games)
                .ThenInclude(g => g.HomeTeam)
                .Include(g => g.ChampionShip)
                .ThenInclude(c => c.Teams)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                throw new ArgumentNullException($"Game with id {id} was not found.");
            }

            game.IsDeleted = true;

            var championShip = game.ChampionShip;

            bool isAwayTeamInAnotherGame = championShip.Games
                                         .Any(g => g.IsDeleted == false
                                              && g != game
                                              && (g.AwayTeam == game.AwayTeam
                                                  || g.HomeTeam == game.AwayTeam));

            if (!isAwayTeamInAnotherGame)
            {
                championShip.Teams.Remove(game.AwayTeam);
            }

            bool isHomeTeamInAnotherGame = championShip.Games
                                         .Any(g => g.IsDeleted == false
                                              && g != game
                                              && (g.AwayTeam == game.HomeTeam
                                                  || g.HomeTeam == game.HomeTeam));

            if (!isHomeTeamInAnotherGame)
            {
                championShip.Teams.Remove(game.HomeTeam);
            }

            await repository.SaveChangesAsync();
        }
    }
}
