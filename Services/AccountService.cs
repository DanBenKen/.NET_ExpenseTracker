using ExpenseTracker.Models.ViewModels.AccountViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using ExpenseTracker.Data;
using ExpenseTracker.Utils.Exceptions;

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
            var passwordValidationResult = await ValidatePasswordAsync(model.Password);
            if (!passwordValidationResult.Succeeded)
            {
                return passwordValidationResult;
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!signInResult.Succeeded)
                {
                    throw new InvalidOperationException("Registration succeeded, but automatic sign-in failed.");
                }
            }

            return result;
        }

        public async Task<IdentityResult> ValidatePasswordAsync(string password)
        {
            var passwordValidators = _userManager.PasswordValidators;
            var user = new ApplicationUser();

            foreach (var validator in passwordValidators)
            {
                var result = await validator.ValidateAsync(_userManager, user, password);
                if (!result.Succeeded)
                    return result;
            }

            return IdentityResult.Success;
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return SignInResult.Failed;

            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return SignInResult.Failed;

            await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

            return SignInResult.Success;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SetOverdraftLimitViewModel> GetOverdraftLimitAsync(string userId)
        {
            var user = await _context.Users.FindAsync(userId)
                        ?? throw new UserNotFoundException(userId);

            return new SetOverdraftLimitViewModel
            {
                AllowedOverdraftLimit = user.AllowedOverdraftLimit ?? 0.00m
            };
        }

        public async Task<bool> SetOverdraftLimitAsync(string userId, decimal allowedOverdraftLimit)
        {
            var user = await _context.Users.FindAsync(userId)
                        ?? throw new UserNotFoundException(userId);

            user.AllowedOverdraftLimit = allowedOverdraftLimit;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
