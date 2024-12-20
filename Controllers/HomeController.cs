﻿using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IHomeService homeService, UserManager<ApplicationUser> userManager)
        {
            _homeService = homeService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = await _homeService.GetHomeIndexDataAsync(userId);

            return View(viewModel);
        }
    }
}
