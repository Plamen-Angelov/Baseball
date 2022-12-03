using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly ITeamService teamService;
        private readonly IChampionShipService championShipService;

        public GameController(IGameService gameService, 
            ITeamService teamService,
            IChampionShipService championShipService)
        {
            this.gameService = gameService;
            this.teamService = teamService;
            this.championShipService = championShipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var teams = await gameService.GetAllAsync();

            return View(teams);
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add()
        {
            var model = new AddGameViewModel();
            model.Teams = await teamService.GetAllTeamNamesAsync();
            model.ChampionShips = await championShipService.GetAllChampionShipNamesAsync();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add(AddGameViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await gameService.AddAsync(model);
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await gameService.GetByIdAsync(id);

            if (game == null)
            {
                return RedirectToAction(nameof(All));
            }

            game.Teams = await teamService.GetAllTeamNamesAsync();
            game.ChampionShips = await championShipService.GetAllChampionShipNamesAsync();

            return View(game);
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id, EditGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id != model.Id)
            {
                return RedirectToAction(nameof(All));
            }

            if (await gameService.GetByIdAsync(id) == null)
            {
                return RedirectToAction(nameof(All));
            }

            await gameService.UpdateAsync(id, model);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await gameService.GetByIdAsync(id) == null)
            {
                return RedirectToAction(nameof(All));
            }

            await gameService.DeleteAsync(id);
            return RedirectToAction(nameof(All));
        }
    }
}
