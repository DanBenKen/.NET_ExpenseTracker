using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.AccountViewModels;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.RegisterAsync(model);
            if (!result.Succeeded)
                return NotFound();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginSuccessful = await _accountService.LoginAsync(model);
            if (!loginSuccessful)
                return NotFound();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SetOverdraftLimit()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = await _accountService.GetOverdraftLimitAsync(userId);
            if (viewModel == null)
                return NotFound();

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetOverdraftLimit(SetOverdraftLimitViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);

            var result = await _accountService.SetOverdraftLimitAsync(userId, model.AllowedOverdraftLimit);
            if (!result)
                return NotFound();

            return RedirectToAction("Index", "Home");
        }
    }
}
