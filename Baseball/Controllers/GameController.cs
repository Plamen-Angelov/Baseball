using Baseball.Common.ViewModels.GameViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
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
        public async Task<IActionResult> Add()
        {
            var model = new AddGameViewModel();
            model.Teams = await teamService.GetAllTeamNamesAsync();
            model.ChampionShips = await championShipService.GetAllChampionShipNamesAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddGameViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await gameService.AddAsync(model);
            return RedirectToAction(nameof(All));
        }
    }
}
