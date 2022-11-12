using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
    public class BatController : Controller
    {
        private readonly IBatService batService;
        private readonly IBatMaterialService batMaterialService;


        public BatController(IBatService batService, IBatMaterialService batMaterialService)
        {
            this.batService = batService;
            this.batMaterialService = batMaterialService;
        }

        [HttpGet]
        public IActionResult All()
        {
            var bats = batService.GetAll();

            return View(bats);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddBatViewModel()
            {
                Materials = batMaterialService.GetAllBatMaterials().ToList()
            };

            return View(model);
        }

        [HttpPost]
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
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Bat wasn't added. {e.Message}");
                return View(model);
            };
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var model = batService.GetById(id);
                model.Materials = batMaterialService.GetAllBatMaterials().ToList();
                return View(model);

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"Bat wasn't added. {e.Message}");
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AddBatViewModel model)
        {
            await batService.UpdateAsync(id, model);
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await batService.DeleteAsync(id);
            return RedirectToAction(nameof(All));
        }
    }
}
