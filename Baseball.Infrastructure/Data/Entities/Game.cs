using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(ChampionShip))]
        public int ChampionShipId { get; set; }

        [Required]
        public ChampionShip ChampionShip { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(HomeTeam))]
        public int HomeTeamId { get; set; }

        [Required]
        public Team HomeTeam { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(AwayTeam))]
        public int AwayTeamId { get; set; }

        [Required]
        public Team AwayTeam { get; set; } = null!;

        [Required]
        [MaxLength(Stadium_MaxLength)]
        public string Stadium { get; set; } = null!;

        public int InningPlayed { get; set; }

        public int HomeTeamRuns { get; set; }

        public int AwayTeamRuns { get; set; }

        [ForeignKey(nameof(Winner))]
        public int? WinnerId { get; set; }

        public Team? Winner { get; set; }

        public int HomeTeamHits { get; set; }

        public int AwayTeamHits { get; set; }

        public int HomeTeamErrors { get; set; }

        public int AwayTeamErrors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
