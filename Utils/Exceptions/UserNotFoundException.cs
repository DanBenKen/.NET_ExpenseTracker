﻿namespace ExpenseTracker.Utils.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string userId) 
            : base($"User with ID {userId} not found.") { }
    }
}