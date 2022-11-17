using System.ComponentModel.DataAnnotations;
using static Baseball.Areas.Autentication.Constants;


namespace Baseball.Areas.Autentication.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(UserName_MaxLegth, MinimumLength = UserName_MinLegth)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(User_Email_MaxLegth, MinimumLength = User_Email_MinLegth)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(User_Password_MaxLegth)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
