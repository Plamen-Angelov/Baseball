﻿using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class PlayerService : IPlayerServicece
    {
        private readonly IRepository repository;

        public PlayerService(IRepository repository)
        {
            this.repository = repository;   
        }

        public async Task AddAsync(AddPlayerViewModel model)
        {
            var player = new Player()
            {
                Name = model.Name,
                Number = model.Number,
                Position = model.Position,
                BatId = model.BatId,
                Glove = model.Glove,
                ThrowHand = model.ThrowHand,
                BattingAverage = model.BattingAverage
            };

            await repository.AddAsync(player);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var player = await GetById(id)
                .SingleAsync();

            player.IsDeleted = true;
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlayerViewModel>> GetAllAsync()
        {
            return await repository.GetAll<Player>()
                .Where(p => p.IsDeleted == false)
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
                .ToListAsync();
        }
        private IQueryable<Player> GetById(int id)
        {
            return repository.GetAll<Player>()
                .Where(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<AddPlayerViewModel> GetByIdAsync(int id)
        {
            return await GetById(id)
                .Select(p => new AddPlayerViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Number = p.Number,
                    Position = p.Position,
                    BatId = p.BatId,
                    Glove = p.Glove,
                    ThrowHand = p.ThrowHand,
                    BattingAverage = p.BattingAverage
                })
                .SingleAsync();
        }

        public async Task UpdateAsync(int id, AddPlayerViewModel model)
        {
            var player = await GetById(id)
                .SingleAsync();

            player.Name = model.Name;
            player.Number = model.Number;
            player.Position = model.Position;
            player.BatId = model.BatId;
            player.Glove = model.Glove;
            player.ThrowHand = model.ThrowHand;
            player.BattingAverage = model.BattingAverage;

            await repository.SaveChangesAsync();
        }
    }
}
