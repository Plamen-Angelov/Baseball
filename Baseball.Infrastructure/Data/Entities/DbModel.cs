using System.ComponentModel.DataAnnotations;

namespace Baseball.Infrastructure.Data.Entities
{
    public class DbModel
    {
        [Key]
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
