using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Baseball.Common.RoleConstants;

namespace Baseball.Controllers
{
    [Authorize]
    public class BatController : Controller
    {
        private readonly IBatService batService;
        private readonly IBatMaterialService batMaterialService;
        private readonly ILogger logger;


        public BatController(IBatService batService, IBatMaterialService batMaterialService, ILogger<BatController> logger)
        {
            this.batService = batService;
            this.batMaterialService = batMaterialService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var bats = await batService.GetAllAsync();
                return View(bats);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(All), ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Add()
        {
            var model = new AddBatViewModel()
            {
                Materials = (await batMaterialService.GetAllBatMaterialsAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Add(AddBatViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await batService.AddAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(Add), ex.Message);
                ModelState.AddModelError("", $"Something went wrong. The bat was not added. Please try again.");
                return View(model);
            };
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var model = await batService.GetByIdAsync(id);
                return View(model);
            }
            catch (ArgumentException ae)
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
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Edit(int id, AddBatViewModel model)
        {
            try
            {
                await batService.UpdateAsync(id, model);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentException ae)
            {
                logger.LogError(nameof(Edit), ae.Message);
                return RedirectToAction(nameof(All)); ;
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Edit), e.Message);
                ModelState.AddModelError("", $"Unexpected error occured. Please try again");
                return View(model);
            }
            
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await batService.DeleteAsync(id);
                return RedirectToAction(nameof(All));
            }
            catch (ArgumentException ae)
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
    }
}
