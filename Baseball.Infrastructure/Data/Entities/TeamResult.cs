using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class TeamResult
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Team_Name_MaxLength)]
        public string TeamName { get; set; } = null!;

        [Required]
        public int Year { get; set; }

        [Required]
        public int WinGames { get; set; }

        [Required]
        public int LoseGames { get; set; }

        public bool IsDeleted { get; set; }
    }
}
