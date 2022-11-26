using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels.GameViewModels
{
    public class AddGameViewModel
    {
        [Required]
        public int ChampionShipId { get; set; }

        public List<ChampionShipNameViewModel> ChampionShips { get; set; } = null!;

        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        public List<TeamNameViewModel> Teams { get; set; } = new List<TeamNameViewModel>();

        [Required]
        [StringLength(Stadium_MaxLength, MinimumLength = Stadium_MinLength)]
        public string Stadium { get; set; } = null!;

        [Range(0, 30)]
        public int InningPlayed { get; set; }

        [Range(0, 100)]
        public int HomeTeamRuns { get; set; }

        [Range(0, 100)]
        public int AwayTeamRuns { get; set; }

        [Range(0, 100)]
        public int HomeTeamHits { get; set; }

        [Range(0, 100)]
        public int AwayTeamHits { get; set; }

        [Range(0, 100)]
        public int HomeTeamErrors { get; set; }

        [Range(0, 100)]
        public int AwayTeamErrors { get; set; }
    }
}
