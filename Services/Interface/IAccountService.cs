using ExpenseTracker.Models.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ExpenseTracker.Services.Interface
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        Task<bool> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<SetOverdraftLimitViewModel?> GetOverdraftLimitAsync(string? userId);
        Task<bool> SetOverdraftLimitAsync(string userId, decimal allowedOverdraftLimit);
    }
}
