﻿using Baseball.Common.ViewModels.ChampionShipViewModels;
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

        public ChampionShipService(IRepository repository)
        {
            this.repository = repository;
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
            return await GetById(id)
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
                        Stadium = g.HomeTeam.Name,
                        InningPlayed = g.InningPlayed,
                        AwayTeamRuns = g.AwayTeamRuns,
                        HomeTeamRuns = g.HomeTeamRuns,
                        AwayTeamHits = g.AwayTeamHits,
                        HomeTeamHits = g.AwayTeamHits,
                        AwayTeamErrors = g.AwayTeamErrors,
                        HomeTeamErrors = g.AwayTeamErrors
                    }).ToList(),
                    Teams = c.Teams
                    .Select(t => new TeamViewModel()
                    {
                        Name = t.Name,
                        WinGames = t.WinGames,
                        LoseGames = t.loseGames
                    })
                    .OrderByDescending(t => t.WinGames)
                    .ThenBy(t => t.LoseGames)
                    .ToList()
                })
                .SingleAsync();
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

        public Task<EditChampionShipViewModel> GetByIdAsync(int id)
        {
            return GetById(id)
                .Select(c => new EditChampionShipViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Year = c.Year
                })
                .SingleAsync();
        }

        private IQueryable<ChampionShip> GetById(int id)
        {
            return repository.GetAll<ChampionShip>()
                .Where(c => c.IsDeleted == false && c.Id == id);
        }

        public async Task UpdateAsync(int id, EditChampionShipViewModel model)
        {
            var championShip = await GetById(id).SingleAsync();

            if (championShip == null)
            {
                throw new NullReferenceException($"Championship with id {id} was not found");
            }

            championShip.Name = model.Name;
            championShip.Year = model.Year;

            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var championShip = await GetById(id)
                .FirstOrDefaultAsync();

            championShip.IsDeleted = true;

            await repository.SaveChangesAsync();
        }
    }
}