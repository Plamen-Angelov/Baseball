
using Microsoft.AspNetCore.Identity;

namespace Baseball.Common.ViewModels.UserViewModels
{
    public class FoundByEmailViewModel
    {
        public IdentityUser User { get; set; } = null!;

        public bool IsInRolePlayer { get; set; }
    }
}
