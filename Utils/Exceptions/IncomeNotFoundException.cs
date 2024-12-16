namespace ExpenseTracker.Utils.Exceptions
{
    public class IncomeNotFoundException : Exception
    {
        public IncomeNotFoundException(int incomeId)
            : base($"Income with ID {incomeId} not found.") { }
    }
}
