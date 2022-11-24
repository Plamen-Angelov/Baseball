using Baseball.Common.ViewModels.TeamViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService teamService;

        public TeamController(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var teams = await teamService.GetAll();

            return View(teams);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddTeamModel();

            return View(model);
        }

        [HttpPost]
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
            catch (Exception)
            {
                ModelState.AddModelError("", $"Something went wrong. Team was not added.");
                return View(model);
            };
        }

        public async Task<IActionResult> GetDetails(int id)
        {
            var teamDetails = await teamService.GetDetailsAsync(id);

            if (teamDetails == null)
            {
                ModelState.AddModelError("", "Team was not found or unexpected error occured.");
                return RedirectToAction(nameof(All));
            }

            return View(teamDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var team = await teamService.GetByIdAsync(id);

            if (team == null)
            {
                return RedirectToAction(nameof(All));
            }

            return View(team);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTeamViewModel model)
        {
            if (id != model.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }

            if (await teamService.GetByIdAsync(id) == null)
            {
                ModelState.AddModelError("", "Team was not found");
                return View(model);
            }

            await teamService.UpdateAsync(id, model);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (await teamService.GetByIdAsync(id) == null)
            {
                return RedirectToAction(nameof(All));
            }


            await teamService.DeleteAsync(id);
            return RedirectToAction(nameof(All));
        }
    }
}
