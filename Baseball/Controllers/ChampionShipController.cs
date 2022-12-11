﻿using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Baseball.Common.RoleConstants;

namespace Baseball.Controllers
{
    [Authorize]
    public class ChampionShipController : Controller
    {
        private readonly IChampionShipService championShipService;
        private readonly ILogger logger;

        public ChampionShipController(IChampionShipService championShipService, ILogger<ChampionShipController> logger)
        {
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
                var championShips = await championShipService.GetAllAsync();
                return View(championShips);
            }
            catch (Exception ex)
            {
                logger.LogError(nameof(All), ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {
            try
            {
                var championShipDetails = await championShipService.GetDetailsAsync(id);
                return View(championShipDetails);
            }
            catch(InvalidOperationException oe)
            {
                logger.LogError(nameof(GetDetails), oe.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(GetDetails), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public IActionResult Add()
        {
            var championShip = new AddChampionShipViewModel();
            return View(championShip);
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Add(AddChampionShipViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await championShipService.AddAsync(model);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Add), e.Message);
                ModelState.AddModelError("", $"Something went wrong. Champion ship was not added. Please try again.");
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var championShip = await championShipService.GetByIdAsync(id);
                if (championShip is null)
                {
                    return RedirectToAction(nameof(All));
                }

                return View(championShip);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Edit), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        [Authorize(Roles = $"{CoachRoleName}, {PlayerRoleName}")]
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
                await championShipService.DeleteAsync(id);
                return RedirectToAction(nameof(All));
            }
            catch(ArgumentNullException ne)
            {
                logger.LogError(nameof(Delete), ne.Message);
                return RedirectToAction(nameof(All));
            }
            catch (Exception e)
            {
                logger.LogError(nameof(Delete), e.Message);
                return RedirectToAction(nameof(All));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetHomePageAll()
        {
            try
            {
                var championShips = await championShipService.GetHomePageAllAsync();
                return View(championShips);
            }
            catch (Exception e)
            {
                logger.LogError(nameof(GetHomePageAll), e.Message);
                return View("Unreachable");
            }
        }
    }
}
