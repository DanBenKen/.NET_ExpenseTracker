﻿using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        public decimal? AllowedOverdraftLimit { get; set; }
    }
}
