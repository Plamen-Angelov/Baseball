using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class BatMaterial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Bat_MaterialName_MaxLength)]
        public string Name { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
