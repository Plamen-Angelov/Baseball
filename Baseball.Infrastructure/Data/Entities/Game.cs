using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ChampionShip_Name_MaxLength)]
        public string ChampionShipName { get; set; } = null!;

        [Required]
        [MaxLength(Team_Name_MaxLength)]
        public string HomeTeamName { get; set; } = null!;

        [Required]
        [MaxLength(Team_Name_MaxLength)]
        public string AwayTeamName { get; set; } = null!;

        [Required]
        [MaxLength(Stadium_MaxLength)]
        public string Stadium { get; set; } = null!;

        public int InningPlayed { get; set; }

        public int HomeTeamRuns { get; set; }

        public int AwayTeamRuns { get; set; }

        public int HomeTeamHits { get; set; }

        public int AwayTeamHits { get; set; }

        public int HomeTeamErrors { get; set; }

        public int AwayTeamErrors { get; set; }

        public bool IsDeleted { get; set; }
    }
}
