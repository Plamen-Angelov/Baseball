using Baseball.Common.ViewModels.PlayerViewModels;
using System.ComponentModel.DataAnnotations;

namespace Baseball.Common.ViewModels.TeamViewModels
{
    public class AddPlayerToTeamViewModel
    {
        public PlayerViewModel? Player { get; set; }

        [Required]
        public int TeamId { get; set; }

        public List<TeamNameViewModel> Teams { get; set; } = new List<TeamNameViewModel>();
    }
}
