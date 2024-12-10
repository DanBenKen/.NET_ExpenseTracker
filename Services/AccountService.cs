using ExpenseTracker.Models.ViewModels.AccountViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interface;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Data;
using System.Security.Claims;

namespace ExpenseTracker.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SetOverdraftLimitViewModel?> GetOverdraftLimitAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new SetOverdraftLimitViewModel
            {
                AllowedOverdraftLimit = user.AllowedOverdraftLimit ?? 0.00m
            };
        }

        public async Task<bool> SetOverdraftLimitAsync(string userId, decimal allowedOverdraftLimit)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.AllowedOverdraftLimit = allowedOverdraftLimit;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
