using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels.GameViewModels
{
    public class EditGameViewModel
    {
        public int Id { get; set; }

        [Required]
        public int ChampionShipId { get; set; }

        public List<ChampionShipNameViewModel> ChampionShips { get; set; } = new List<ChampionShipNameViewModel>();

        [Required]
        public int HomeTeamId { get; set; }

        [Required]
        public int AwayTeamId { get; set; }

        public List<TeamNameViewModel> Teams { get; set; } = new List<TeamNameViewModel>();

        [Required]
        [StringLength(Stadium_MaxLength, MinimumLength = Stadium_MinLength)]
        public string Stadium { get; set; } = null!;

        [Range(Game_InningsPlayed_MinValue, Game_InningsPlayed_MaxValue)]
        public int InningPlayed { get; set; }

        [Range(Game_Runs_MinValue, Game_Runs_MaxValue)]
        public int HomeTeamRuns { get; set; }

        [Range(Game_Runs_MinValue, Game_Runs_MaxValue)]
        public int AwayTeamRuns { get; set; }

        [Range(Game_Hits_MinValue, Game_Hits_MaxValue)]
        public int HomeTeamHits { get; set; }

        [Range(Game_Hits_MinValue, Game_Hits_MaxValue)]
        public int AwayTeamHits { get; set; }

        [Range(Game_Errors_MinValue, Game_Errors_MaxValue)]
        public int HomeTeamErrors { get; set; }

        [Range(Game_Errors_MinValue, Game_Errors_MaxValue)]
        public int AwayTeamErrors { get; set; }
    }
}
