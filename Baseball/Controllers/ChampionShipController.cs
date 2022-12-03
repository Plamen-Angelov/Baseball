using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    [Authorize]
    public class ChampionShipController : Controller
    {
        private readonly IChampionShipService championShipService;

        public ChampionShipController(IChampionShipService championShipService)
        {
            this.championShipService = championShipService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var championShips = await championShipService.GetAllAsync();

            return View(championShips);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            if (await championShipService.GetByIdAsync(id) == null)
            {
                return RedirectToAction(nameof(All));
            }

            var championShipDetails = await championShipService.GetDetailsAsync(id);
            return View(championShipDetails);
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public IActionResult Add()
        {
            var championShip = new AddChampionShipViewModel();

            return View(championShip);
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Add(AddChampionShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await championShipService.AddAsync(model);
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id)
        {
            var championShip = await championShipService.GetByIdAsync(id);

            return View(championShip);
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Edit(int id, EditChampionShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (id != model.Id)
            {
                return View(model);
            }

            try
            {
                await championShipService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coach, Player")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await championShipService.GetByIdAsync(id) == null)
            {
                return RedirectToAction(nameof(All));
            }

            await championShipService.DeleteAsync(id);
            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHomePageAll()
        {
            var championShips = await championShipService.GetHomePageAllAsync();
            return View(championShips);
        }
    }
}
