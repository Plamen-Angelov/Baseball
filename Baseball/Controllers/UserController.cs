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
        private readonly ILogger logger;

        public UserController(IUserService userService, UserManager<IdentityUser> userManager, ILogger<UserController> logger)
        {
            this.userService = userService;
            this.userManager = userManager;
            this.logger = logger;
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

            try
            {
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
            catch (Exception e)
            {
                logger.LogError(nameof(GetByEmail), e.Message);
                ModelState.AddModelError("", $"Unexpected error occured. Please try again");
                return View(model);
            } 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersWithPlayerRole()
        {
            try
            {
                var players = await userManager.GetUsersInRoleAsync("Player");
                return View(players);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(GetAllUsersWithPlayerRole), e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddToPlayers(string id)
        {
            try
            {
                var user = await userManager.FindByIdAsync(id);

                await userManager.AddToRolesAsync(user, new string[] { "Player" });

                return RedirectToAction(nameof(GetAllUsersWithPlayerRole));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(AddToPlayers), e.Message);
                return RedirectToAction(nameof(GetAllUsersWithPlayerRole));
            }
        }
    }
}
