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

        public PlayerController(
            IPlayerServicece playerService, 
            IBatService batService,
            ITeamService teamService)
        {
            this.playerService = playerService;
            this.batService = batService;
            this.teamService = teamService;
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
        [Authorize(Roles = "Coach, Player")]
        public IActionResult Add()
        {
            var model = new AddPlayerViewModel()
            {
                Bats = batService.GetAll()
            };

            return View(model);
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong. Player was not added");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
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
        [Authorize(Roles = "Coach, Player")]
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

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(User.IsInRole("Player") || User.IsInRole("Coach")))
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            await playerService.DeleteAsync(id);

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> AddToTeam(int id)
        {
            var model = new AddPlayerToTeamViewModel()
            {
                Player = await playerService.GetPlayerByIdAsync(id),
                Teams = await teamService.GetAllTeamNamesAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> AddToTeam(int id, AddPlayerToTeamViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await playerService.AddToTeamAsync(id, model.TeamId);

            return RedirectToAction(nameof(All));
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
            catch(NullReferenceException ex)
            {
                return RedirectToAction("GetDetails", "Team");

            }
            catch (Exception)
            {
                return RedirectToAction("GetDetails", "Team");

            }
        }
    }
}
