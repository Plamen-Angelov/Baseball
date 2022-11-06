using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Infrastructure.Data.Entities
{
    public class ChampionShip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ChampionShip_Name_MaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int Year { get; set; }

        public ICollection<TeamResult> TeamResults { get; set; }

        public ICollection<Game> Games { get; set; }

        public bool IsDeleted { get; set; }


        public ChampionShip()
        {
            TeamResults = new List<TeamResult>();
            Games = new List<Game>();
        }
    }
}
