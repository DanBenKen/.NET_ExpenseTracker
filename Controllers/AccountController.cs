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

            var passwordValidationResult = await _accountService.ValidatePasswordAsync(model.Password);
            if (!passwordValidationResult.Succeeded)
            {
                foreach (var error in passwordValidationResult.Errors)
                {
                    ModelState.AddModelError(nameof(model.Password), error.Description);
                }
                return View(model);
            }

            var result = await _accountService.RegisterAsync(model);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }


        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.LoginAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

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
            //if (string.IsNullOrEmpty(userId))
            //    return Unauthorized();            //Defanzivno programiranje.

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
            {
                ModelState.AddModelError("", "Unable to set the overdraft limit. Please try again.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
