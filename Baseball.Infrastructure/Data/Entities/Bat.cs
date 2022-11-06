using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Bat
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public BatMaterial Material { get; set; }

        [Required]
        [MaxLength(Bat_Brand_MaxLength)]
        public string Brand { get; set; } = null!;

        [Required]
        public int Size { get; set; }
    }
}
