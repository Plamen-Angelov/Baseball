using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Glove
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Bat_Brand_MaxLength)]
        public string Brand { get; set; } = null!;

        [Required]
        public int Size { get; set; }

        [MaxLength(Glove_Color_MaxLength)]
        public string Color { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
