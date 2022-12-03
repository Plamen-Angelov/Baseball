using Baseball.Common.ViewModels.TeamViewModels;

namespace Baseball.Common.ViewModels.ChampionShipViewModels
{
    public class HomePageViewModel
    {
        public string ChampionShipName { get; set; } = null!;

        public int ChampionShipYear { get; set; }

        public List<TeamScoreViewModel> Teams { get; set; } = new List<TeamScoreViewModel>();
    }
}
