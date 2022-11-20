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

        public DbSet<Bat> Bats { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<TeamResult> TeamResults { get; set; }

        public DbSet<BatMaterial> BatMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BatMaterial>()
                .HasData(BatMaterialSeed());

            base.OnModelCreating(builder);
        }

        private List<BatMaterial> BatMaterialSeed()
        {
            return new List<BatMaterial>()
            {
                new BatMaterial()
                {
                    Id = 1,
                    Name = "Wood"
                },
                new BatMaterial()
                {
                    Id = 2,
                    Name = "Aluminium"
                }
            };
        }
    }
}