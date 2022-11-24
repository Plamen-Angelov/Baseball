using Baseball.Common.ViewModels.PlayerViewModels;

namespace Baseball.Common.ViewModels.TeamViewModels
{
    public class TeamDetailsViewModel
    {
        public string Name { get; set; } = null!;

        public string HomeColor { get; set; } = null!;

        public string AwayColor { get; set; } = null!;

        public int WinGames { get; set; }

        public int loseGames { get; set; }

        public List<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    }
}
