namespace Baseball.Common.ViewModels.TeamViewModels
{
    public class TeamViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int WinGames { get; set; }

        public int LoseGames { get; set; }
    }
}
