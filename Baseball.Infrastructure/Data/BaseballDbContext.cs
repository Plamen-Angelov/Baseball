using Baseball.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Infrastructure.Data
{
    public class BaseballDbContext : IdentityDbContext
    {
        public BaseballDbContext(DbContextOptions<BaseballDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChampionShip> ChampionShips { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Glove> Gloves { get; set; }

        public DbSet<Bat> Bats { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamResult> TeamResults { get; set; }
    }
}