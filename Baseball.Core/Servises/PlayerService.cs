using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Core.Contracts;
using Baseball.Infrastructure.Data.Entities;
using Baseball.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Core.Servises
{
    public class PlayerService : IPlayerServicece
    {
        private readonly IRepository repository;
        private readonly ITeamService teamService;

        public PlayerService(IRepository repository, ITeamService teamService)
        {
            this.repository = repository;
            this.teamService = teamService;
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
                .FirstOrDefaultAsync();

            if (player == null)
            {
                throw new ArgumentException("Player was not found");
            }

            player.IsDeleted = true;
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlayerViewModel>> GetAllAsync(string? teamName = null, string? searchText = null, PlayerSorting? sorting = null)
        {
            var players = repository.GetAll<Player>()
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
                    BattingAverageDouble = p.BattingAverage,
                    BattingAverage = p.BattingAverage.ToString("F3"),
                    TeamName = p.Team.Name
                });

            if (string.IsNullOrEmpty(teamName) == false)
            {
                players = players
                    .Where(p => p.TeamName == teamName);
            }

            if (string.IsNullOrEmpty(searchText) == false)
            {
                var searchBy = searchText.Split(new string[] {",", ", ", " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var search in searchBy)
                {
                    var searchTerm = $"%{search.ToLower()}%";

                    players = players
                        .Where(p => EF.Functions.Like(p.Position.ToLower(), searchTerm));
                }
            }

            if (string.IsNullOrEmpty(sorting.ToString()) == false)
            {
                players = sorting switch
                {
                    PlayerSorting.NoSorting => players,
                    PlayerSorting.BatingAvgDesc => players
                    .OrderByDescending(p => p.BattingAverageDouble),
                    PlayerSorting.BatingAvgAsc => players
                    .OrderBy(p => p.BattingAverageDouble),
                    PlayerSorting.NameAsc => players
                    .OrderBy(p => p.Name),
                    PlayerSorting.NameDesc => players
                    .OrderByDescending(p => p.Name)
                };
            }

            return await players.ToListAsync();
        }

        private IQueryable<Player> GetById(int id)
        {
            return repository.GetAll<Player>()
                .Where(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<AddPlayerViewModel?> GetByIdAsync(int id)
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
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, AddPlayerViewModel model)
        {
            var player = await GetById(id)
                .FirstOrDefaultAsync();

            if (player == null)
            {
                throw new ArgumentException($"Player with id {id} was not found");
            }

            player.Name = model.Name;
            player.Number = model.Number;
            player.Position = model.Position;
            player.BatId = model.BatId;
            player.Glove = model.Glove;
            player.ThrowHand = model.ThrowHand;
            player.BattingAverage = model.BattingAverage;

            await repository.SaveChangesAsync();
        }

        public async Task<PlayerViewModel?> GetPlayerByIdAsync(int id)
        {
            return await GetById(id)
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
                .FirstOrDefaultAsync();
        }

        public async Task AddToTeamAsync(int playerId, int teamId)
        {
            var player = await GetById(playerId).FirstOrDefaultAsync();

            if (player == null)
            {
                throw new NullReferenceException("Player was not found");
            }

            var team = await teamService.GetEntityByIdAsync(teamId);

            if (team == null)
            {
                throw new NullReferenceException("Team was not found");
            }
            
            team.Players.Add(player);
            await repository.SaveChangesAsync();
        }

        public async Task<int?> MakePlayerFreeAgentAsync(int playerId)
        {
            var player = await GetById(playerId)
                .Include(p => p.Team)
                .FirstOrDefaultAsync();

            if (player == null)
            {
                throw new NullReferenceException("Player was not found");
            }

            if (player.Team == null)
            {
                throw new NullReferenceException("Player's team was not found");
            }

            var teamId = player.TeamId;
            player.Team!.Players.Remove(player);
            await repository.SaveChangesAsync();

            return teamId;
        }
    }
}
