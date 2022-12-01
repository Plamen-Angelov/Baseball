﻿using Baseball.Common.ViewModels.PlayerViewModels;

namespace Baseball.Core.Contracts
{
    public interface IPlayerServicece
    {
        Task <IEnumerable<PlayerViewModel>> GetAllAsync();

        Task AddAsync(AddPlayerViewModel model);

        Task<AddPlayerViewModel> GetByIdAsync(int id);

        Task<PlayerViewModel> GetPlayerByIdAsync(int id);

        Task UpdateAsync(int id, AddPlayerViewModel model);

        Task DeleteAsync(int id);

        Task AddToTeamAsync(int playerId, int teamId);
    }
}
