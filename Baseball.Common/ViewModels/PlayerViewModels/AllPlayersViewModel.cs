using Baseball.Common.ViewModels.TeamViewModels;

namespace Baseball.Common.ViewModels.PlayerViewModels
{
    public class AllPlayersViewModel
    {
        public string? TeamName { get; set; }

        public IEnumerable<TeamNameViewModel>? TeamNames { get; set; } = new List<TeamNameViewModel>();

        public PlayerSorting Sorting { get; set; }

        public string? SearchText { get; set; }

        public IEnumerable<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
