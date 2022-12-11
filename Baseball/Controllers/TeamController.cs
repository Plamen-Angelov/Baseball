using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Baseball.Common.RoleConstants;

namespace Baseball.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;
        private readonly ILogger logger;

        public TeamController(ITeamService teamService, ILogger<TeamController> logger)
        {
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
                var teams = await teamService.GetAllAsync();
                return View(teams);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(All), e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public IActionResult Add()
        {
            var model = new AddTeamModel();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Add(AddTeamModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await teamService.AddAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Add), e.Message);
                ModelState.AddModelError("", $"Something went wrong. Team was not added.");
                return View(model);
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var teamDetails = await teamService.GetDetailsAsync(id);

                if (teamDetails == null)
                {
                    ModelState.AddModelError("", "Team was not found or unexpected error occured.");
                    return RedirectToAction(nameof(All));
                }

                return View(teamDetails);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(GetDetails), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var team = await teamService.GetByIdAsync(id);

                if (team == null)
                {
                    return RedirectToAction(nameof(All));
                }

                return View(team);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Edit), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Edit(int id, EditTeamViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToAction(nameof(All));
            }

            try
            {
                await teamService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentNullException ne)
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
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await teamService.DeleteAsync(id);
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
