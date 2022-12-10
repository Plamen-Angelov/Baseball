using Baseball.Common.ViewModels.PlayerViewModels;
using Baseball.Common.ViewModels.TeamViewModels;
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
        private readonly ITeamService teamService;
        private readonly ILogger logger;

        public PlayerController(
            IPlayerServicece playerService,
            IBatService batService,
            ITeamService teamService,
            ILogger<PlayerController> logger)
        {
            this.playerService = playerService;
            this.batService = batService;
            this.teamService = teamService;
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
                var players = await playerService.GetAllAsync();
                return View(players);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(All), e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add()
        {
            try
            {
                var model = new AddPlayerViewModel()
                {
                    Bats = await batService.GetAllAsync()
                };

                return View(model);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Add), e.Message);
                return RedirectToAction(nameof(All));
            }

        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
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
            catch (Exception e)
            {
                logger.LogError(nameof(Add), e.Message);
                ModelState.AddModelError("", "Something went wrong. Player was not added. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var player = await playerService.GetByIdAsync(id);

                if (player == null)
                {
                    return RedirectToAction(nameof(All));
                }

                player.Bats = await batService.GetAllAsync();

                return View(player);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Edit), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id, AddPlayerViewModel model)
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
                await playerService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentException ae)
            {
                logger.LogError(nameof(Edit), ae.Message);
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
                await playerService.DeleteAsync(id);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentException ae)
            {
                logger.LogError(nameof(Delete), ae.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Delete), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> AddToTeam(int id)
        {
            try
            {
                var player = await playerService.GetPlayerByIdAsync(id);

                if (player == null)
                {
                    logger.LogError(nameof(AddToTeam), $"Player with id {id} was not found.");
                    return RedirectToAction(nameof(All));
                }

                var model = new AddPlayerToTeamViewModel()
                {
                    Player = player,
                    Teams = await teamService.GetAllTeamNamesAsync()
                };

                return View(model);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(AddToTeam), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> AddToTeam(int id, AddPlayerToTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await playerService.AddToTeamAsync(id, model.TeamId);
                return RedirectToAction(nameof(All));
            }
            catch(NullReferenceException ne)
            {
                logger.LogError(nameof(AddToTeam), ne.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(AddToTeam), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> MakePlayerFreeAgent(int id)
        {
            try
            {
                var playersTeamId = await playerService.MakePlayerFreeAgentAsync(id);
                return RedirectToAction("GetDetails", "Team", new { id = (int)playersTeamId });
            }
            catch (NullReferenceException ne)
            {
                logger.LogError(nameof(MakePlayerFreeAgent), ne.Message);
                return RedirectToAction("All", "Team");
            }
            catch (Exception e)
            {
                logger.LogError(nameof(MakePlayerFreeAgent), e.Message);
                return RedirectToAction("All", "Team");
            }
        }
    }
}
