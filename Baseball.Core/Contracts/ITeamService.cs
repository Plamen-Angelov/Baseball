﻿using Baseball.Common.ViewModels.TeamViewModels;

namespace Baseball.Core.Contracts
{
    public interface ITeamService
    {
        Task<IEnumerable<TeamViewModel>> GetAll();

        Task AddAsync(AddTeamModel model);

        Task<TeamDetailsViewModel> GetDetailsAsync(int id);

        Task<EditTeamViewModel?> GetByIdAsync(int id);

        Task UpdateAsync(int id, EditTeamViewModel model);

        Task DeleteAsync(int id);
    }
}