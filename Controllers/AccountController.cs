using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.AccountViewModels;
using ExpenseTracker.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;

        public AccountController(IAccountService accountService, UserManager<User> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(model);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginSuccessful = await _accountService.LoginAsync(model);

                if (loginSuccessful)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
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

            if (userId == null)
            {
                return Unauthorized();
            }

            var viewModel = await _accountService.GetOverdraftLimitAsync(userId);

            if (viewModel == null)
            {
                return NotFound();
            }

            Console.WriteLine($"User found: {viewModel?.AllowedOverdraftLimit}");

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetOverdraftLimit(SetOverdraftLimitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _accountService.SetOverdraftLimitAsync(userId, model.AllowedOverdraftLimit);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
