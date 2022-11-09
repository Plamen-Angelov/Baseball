using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Bat : DbModel
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(BatMaterial))]
        public int BatMaterialId { get; set; }

        [Required]
        public BatMaterial BatMaterial { get; set; } = null!;

        [Required]
        [MaxLength(Bat_Brand_MaxLength)]
        public string Brand { get; set; } = null!;

        [Required]
        public int Size { get; set; }

        //public bool IsDeleted { get; set; }
    }
}
