using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Player_Name_MaxLength)]
        public string Name { get; set; } = null!;

        public int Number { get; set; }

        [Required]
        [MaxLength(Player_Position_MaxLength)]
        public string Position { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Bat))]
        public int BatId { get; set; }

        [Required]
        public Bat Bat { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Glove))]
        public int GloveId { get; set; }

        [Required]
        public Glove Glove { get; set; } = null!;

        public string ThrowHand { get; set; } = null!;

        [Required]
        public double BattingAverage { get; set; }

        public bool IsDeleted { get; set; }
    }
}
