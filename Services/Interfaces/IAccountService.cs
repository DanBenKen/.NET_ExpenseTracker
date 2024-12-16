using ExpenseTracker.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<SetOverdraftLimitViewModel> GetOverdraftLimitAsync(string userId);
        Task<bool> SetOverdraftLimitAsync(string userId, decimal allowedOverdraftLimit);
        Task<IdentityResult> ValidatePasswordAsync(string password);
    }
}
