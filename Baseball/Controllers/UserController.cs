using Baseball.Common.ViewModels.UserViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<IdentityUser> userManager;

        public UserController(IUserService userService, UserManager<IdentityUser> userManager)
        {
            this.userService = userService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetByEmail()
        {
            var model = new GetByEmailViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetByEmail(GetByEmailViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            
            return View("FoundByEmail", user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersWithPlayerRole()
        {
            var players = await userManager.GetUsersInRoleAsync("Player");

            return View(players);
        }

        [HttpGet]
        public async Task<IActionResult> AddToPlayers(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            await userManager.AddToRolesAsync(user, new string[] { "Player" });

            return RedirectToAction(nameof(GetAllUsersWithPlayerRole));
        }
    }
}
