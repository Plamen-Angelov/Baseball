namespace Baseball.Common.ViewModels.PlayerViewModels
{
    public class PlayerViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int Number { get; set; }

        public string Position { get; set; } = null!;

        public string Bat { get; set; } = null!;

        public string Glove { get; set; } = null!;

        public string ThrowHand { get; set; } = null!;

        public string BattingAverage { get; set; } = null!;
    }
}
