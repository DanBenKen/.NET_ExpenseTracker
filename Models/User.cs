using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models
{
    public class User : IdentityUser
    {
        public decimal? AllowedOverdraftLimit { get; set; }
    }
}
