using Baseball.Common.ViewModels;
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
    }
}
