namespace Baseball.Common.ViewModels.GameViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }

        public string ChampionShip { get; set; } = null!;

        public string HomeTeamName { get; set; } = null!;

        public string AwayTeamName { get; set; } = null!;

        public string Stadium { get; set; } = null!;

        public int InningPlayed { get; set; }

        public int HomeTeamRuns { get; set; }

        public int AwayTeamRuns { get; set; }

        public int HomeTeamHits { get; set; }

        public int AwayTeamHits { get; set; }

        public int HomeTeamErrors { get; set; }

        public int AwayTeamErrors { get; set; }
    }
}
