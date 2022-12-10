using Baseball.Common.ViewModels.BatViewModels;
using Baseball.Controllers;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Baseball.Tests.ControllerTests
{
    public class BatControllerTests
    {
        private Mock<IBatService> mockBatService;
        private Mock<IBatMaterialService> mockBatMaterialService;
        private Mock<ILogger<BatController>> logger;
        private BatController batController;

        [SetUp]
        public void Setup()
        {
            mockBatService = new Mock<IBatService>();
            mockBatMaterialService = new Mock<IBatMaterialService>();
            logger = new Mock<ILogger<BatController>>();
            batController = new BatController(mockBatService.Object, mockBatMaterialService.Object, logger.Object);
        }

        [Test]
        public async Task All_ShouldReturnCorrectData()
        {
            List<BatViewModel> bats = new List<BatViewModel>()
            {
                new BatViewModel()
                {
                    id = 1,
                    Material = "wood",
                    Brand = "Academa",
                    Size = 33
                },
                new BatViewModel()
                {
                    id = 2,
                    Material = "aluminium",
                    Brand = "Academa",
                    Size = 26
                },
                new BatViewModel()
                {
                    id = 3,
                    Material = "wood",
                    Brand = "E7",
                    Size = 33
                }
            };

            mockBatService.Setup(s => s.GetAllAsync()).ReturnsAsync(bats);

            var result = await batController.All() as ViewResult;
            var batViews = (List<BatViewModel>?)result?.ViewData.Model;

            Assert.IsNotNull(result);
            Assert.IsTrue(batViews?.Count == 3);
            Assert.IsTrue(batViews?.FirstOrDefault()?.id == 1);
            mockBatService.VerifyAll();
        }

        [Test]
        public async Task Add_ShouldCallServiceOnce()
        {
            
            mockBatService.Setup(s => s.AddAsync(It.IsAny<AddBatViewModel>()));

            await batController.Add(It.IsAny<AddBatViewModel>());

            mockBatService.Verify( x => x.AddAsync(It.IsAny<AddBatViewModel>()), Times.Once());
        }

        [Test]
        public async Task Add_ShouldreturnRedirectToAction_WhenAddIsSuccessful()
        {
            mockBatService.Setup(s => s.AddAsync(It.IsAny<AddBatViewModel>()));

             var result = await batController.Add(It.IsAny<AddBatViewModel>());

            Assert.IsTrue(result.GetType() == typeof(RedirectToActionResult));
        }
    }
}
