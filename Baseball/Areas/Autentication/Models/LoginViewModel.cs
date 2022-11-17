using System.ComponentModel.DataAnnotations;

namespace Baseball.Areas.Autentication.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
