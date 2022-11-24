using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Team_Name_MaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(Team_Color_MaxLength)]
        public string HomeColor { get; set; } = null!;

        [MaxLength(Team_Color_MaxLength)]
        public string AwayColor { get; set; } = null!;

        public int WinGames { get; set; }

        public int loseGames { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();

        public bool IsDeleted { get; set; }
    }
}
