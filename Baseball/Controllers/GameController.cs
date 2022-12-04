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
        private readonly ILogger logger;

        public GameController(IGameService gameService, 
            ITeamService teamService,
            IChampionShipService championShipService,
            ILogger<GameController> logger)
        {
            this.gameService = gameService;
            this.teamService = teamService;
            this.championShipService = championShipService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var teams = await gameService.GetAllAsync();
                return View(teams);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(All), ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add()
        {
            try
            {
                var model = new AddGameViewModel();
                model.Teams = await teamService.GetAllTeamNamesAsync();
                model.ChampionShips = await championShipService.GetAllChampionShipNamesAsync();

                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Add), ex.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add(AddGameViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await gameService.AddAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentNullException ne)
            {
                logger.LogError(nameof(Add), ne.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Add), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var game = await gameService.GetByIdAsync(id);

                if (game == null)
                {
                    logger.LogError(nameof(Edit), "Game was not found");
                    return RedirectToAction(nameof(All));
                }

                game.Teams = await teamService.GetAllTeamNamesAsync();
                game.ChampionShips = await championShipService.GetAllChampionShipNamesAsync();

                return View(game);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Edit), ex.Message);
                return RedirectToAction(nameof(All));
            }
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

            try
            {
                await gameService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentNullException ne)
            {
                logger.LogError(nameof(Edit), ne.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Edit), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await gameService.DeleteAsync(id);
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentNullException ne)
            {
                logger.LogError(nameof(Delegate), ne.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Delete), e.Message);
                return RedirectToAction(nameof(All));
            }
        }
    }
}
