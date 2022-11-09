using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels
{
    public class AddBatViewModel
    {
        [Required]
        [StringLength(Bat_Brand_MaxLength, MinimumLength = Bat_Brand_MinLength)]
        public string Brand { get; set; } = null!;

        [Required]
        [Range(Bat_Size_Min, Bat_Size_Max)]
        public int Size { get; set; }

        [Required]
        public int MaterialId { get; set; }

        public List<BatMaterialViewModel> Materials { get; set; }
    }
}
