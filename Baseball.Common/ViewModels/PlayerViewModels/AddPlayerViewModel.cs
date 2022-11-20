using Baseball.Common.ViewModels.BatViewModels;
using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels.PlayerViewModels
{
    public class AddPlayerViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(Player_Name_MaxLength, MinimumLength = Player_Name_MinLength)]
        public string Name { get; set; } = null!;

        [Range(0, 100)]
        public int Number { get; set; }

        [Required]
        [MaxLength(Player_Position_MaxLength)]
        public string Position { get; set; } = null!;

        [Required]
        public int BatId { get; set; }

        public IEnumerable<BatViewModel>? Bats { get; set; }

        [Required]
        [StringLength(Player_Glove_MaxLength, MinimumLength = Player_Glove_MinLength)]
        public string Glove { get; set; } = null!;

        [Required]
        public string ThrowHand { get; set; } = null!;

        [Required]
        [Range(typeof(double), "0", "1")]
        public double BattingAverage { get; set; }
    }
}
