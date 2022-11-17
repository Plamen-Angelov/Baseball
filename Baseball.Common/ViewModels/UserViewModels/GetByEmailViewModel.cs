using System.ComponentModel.DataAnnotations;
using static Baseball.Common.Constants;

namespace Baseball.Common.ViewModels.UserViewModels
{
    public class GetByEmailViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(User_Email_MaxLegth)]
        public string Email { get; set; } = null!;
    }
}
