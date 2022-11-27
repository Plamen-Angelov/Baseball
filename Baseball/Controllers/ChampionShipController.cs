﻿using Baseball.Common.ViewModels.ChampionShipViewModels;
using Baseball.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Baseball.Controllers
{
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
        public IActionResult Add()
        {
            var championShip = new AddChampionShipViewModel();

            return View(championShip);
        }

        [HttpPost]
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
        public async Task<IActionResult> Edit(int id)
        {
            var championShip = await championShipService.GetByIdAsync(id);

            return View(championShip);
        }

        [HttpPost]
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
    }
}
