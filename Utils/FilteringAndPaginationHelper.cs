namespace ExpenseTracker.Utils
{
    public class FilteringAndPaginationHelper
    {
        public static IQueryable<T> ApplyFilters<T>(IQueryable<T> query, Func<T, bool> additionalFilter)
        {
            return query.Where(e => additionalFilter(e)).AsQueryable();
        }

        public static async Task<PaginatedList<T>> PaginateAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            return await PaginatedList<T>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
