using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly IPlayerServicece playerService;
        private readonly IBatService batService;

        public PlayerController(
            IPlayerServicece playerService, 
            IBatService batService)
        {
            this.playerService = playerService;
            this.batService = batService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var players = await playerService.GetAllAsync();

            return View(players);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddPlayerViewModel()
            {
                Bats = batService.GetAll()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPlayerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await playerService.AddAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong. Player was not added");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var player = await playerService.GetByIdAsync(id);

            if (player == null)
            {
                return RedirectToAction(nameof(All));
            }

            if (!(User.IsInRole("Player") || User.IsInRole("Coach")))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            player.Bats = batService.GetAll();

            return View(player);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddPlayerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!(User.IsInRole("Player") || User.IsInRole("Coach")))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (id != model.Id)
            {
                return RedirectToAction(nameof(All));
            }

            try
            {
                await playerService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Player was not found or unexpected error occured");
                return View(model);
            }
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!(User.IsInRole("Player") || User.IsInRole("Coach")))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await playerService.DeleteAsync(id);

            return RedirectToAction(nameof(All));
        }
    }
}
