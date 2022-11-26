using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels.ChampionShipViewModels
{
    public class AddChampionShipViewModel
    {
        [Required]
        [StringLength(ChampionShip_Name_MaxLength, MinimumLength = ChampionShip_Name_MinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ChampionShip_Year_MinValue, ChampionShip_Year_MaxValue)]
        public int Year { get; set; }
    }
}
