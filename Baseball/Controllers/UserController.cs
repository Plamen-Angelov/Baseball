using Baseball.Common.ViewModels.UserViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    [Authorize(Roles = "Coach")]
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", $"User with email \"{model.Email}\" was not found.");

                return View(model);
            }

            var foundbyEmailModel = new FoundByEmailViewModel()
            {
                User = user
            };

            if ((await userManager.GetRolesAsync(user)).Any(r => r == "Player"))
            {
                foundbyEmailModel.IsInRolePlayer = true;
            }
            
            return View("FoundByEmail", foundbyEmailModel);
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
