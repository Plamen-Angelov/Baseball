using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;


namespace Baseball.Common.ViewModels.TeamViewModels
{
    public class EditTeamViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(Team_Name_MaxLength, MinimumLength = Team_Name_MinLength)]
        public string Name { get; set; } = null!;

        [StringLength(Team_Color_MaxLength, MinimumLength = Team_Color_MinLength)]
        public string HomeColor { get; set; } = null!;

        [StringLength(Team_Color_MaxLength, MinimumLength = Team_Color_MinLength)]
        public string AwayColor { get; set; } = null!;
    }
}
