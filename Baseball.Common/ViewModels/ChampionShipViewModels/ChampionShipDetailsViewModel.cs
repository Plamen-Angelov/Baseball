using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Common.ViewModels.TeamViewModels;

namespace Baseball.Common.ViewModels.ChampionShipViewModels
{
    public class ChampionShipDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Year { get; set; }

        public List<GameViewModel> Games { get; set; } = new List<GameViewModel>();

        public List<TeamViewModel> Teams { get; set; } = new List<TeamViewModel>();
    }
}
