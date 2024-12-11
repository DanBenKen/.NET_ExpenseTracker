using ExpenseTracker.Models.ViewModels.IncomeViewModels;
using ExpenseTracker.Models;
using ExpenseTracker.Utils;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.Services
{
    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly SourcesHelper _sourcesHelper;
        private readonly MonthsHelper _monthsHelper;

        public IncomeService(SourcesHelper helperMethods, ApplicationDbContext context, MonthsHelper monthsHelper) 
        {
            _sourcesHelper = helperMethods;
            _context = context;
            _monthsHelper = monthsHelper;
        }

        public async Task<IncomeIndexViewModel> GetIncomesViewModelAsync(string userId, int? month, int? year, string? source, int pageNumber, int pageSize, bool showAll)
        {
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;
            source ??= "all_sources";

            var sourcesList = _sourcesHelper.GetSources();
            var hasSource = !string.IsNullOrEmpty(source) && source != "all_sources";

            var incomesQuery = _context.Incomes
                .Where(i => i.UserId == userId)
                .Where(i => i.Date.Month == month && i.Date.Year == year);

            if (hasSource)
            {
                incomesQuery = incomesQuery.Where(i => i.Source == source);
            }

            if (showAll)
            {
                var incomes = await incomesQuery.ToListAsync();

                return MapToIncomeIndexViewModel(incomes, sourcesList, month.Value, year.Value, source, pageSize, showAll, null);
            }

            var paginatedIncomes = await PaginatedList<Income>.CreateAsync(incomesQuery, pageNumber, pageSize);

            return MapToIncomeIndexViewModel(paginatedIncomes.Items, sourcesList, month.Value, year.Value, source, pageSize, showAll, paginatedIncomes);
        }

        public IncomeCreateViewModel GetCreateIncomeViewModel(string userId)
        {
            var sources = _sourcesHelper.GetSources();

            return new IncomeCreateViewModel
            {
                Amount = 0,
                Date = DateTime.Now,
                Source = string.Empty,
                Description = string.Empty,
                Sources = sources
            };
        }

        public async Task<bool> CreateIncomeAsync(IncomeCreateViewModel viewModel, string userId)
        {
            var income = new Income
            {
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Source = viewModel.Source,
                Description = viewModel.Description,
                UserId = userId
            };

            _context.Incomes.Add(income);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IncomeCreateViewModel?> GetIncomeForEditAsync(int id, string userId)
        {
            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (income == null)
                return null;

            return new IncomeCreateViewModel
            {
                Amount = income.Amount,
                Date = income.Date,
                Source = income.Source,
                Description = income.Description,
                Sources = _sourcesHelper.GetSources()
            };
        }

        public async Task<bool> UpdateIncomeAsync(int id, string userId, IncomeCreateViewModel viewModel)
        {
            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            if (income == null)
                return false;

            income.Amount = viewModel.Amount;
            income.Date = viewModel.Date;
            income.Source = viewModel.Source;
            income.Description = viewModel.Description;

            _context.Incomes.Update(income);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteIncomeAsync(int id, string userId)
        {
            var income = await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            if (income == null)
                return false;

            _context.Incomes.Remove(income);
            await _context.SaveChangesAsync();

            return true;
        }

        private IncomeIndexViewModel MapToIncomeIndexViewModel(IEnumerable<Income> incomes, List<SelectListItem> sourcesList, int month, int year, string source, int pageSize, bool showAll, PaginatedList<Income>? paginatedList)
        {
            var mappedIncomes = incomes.Select(i => new Income
            {
                Id = i.Id,
                Amount = i.Amount,
                Date = i.Date,
                Description = i.Description,
                Source = sourcesList.FirstOrDefault(s => s.Value == i.Source)?.Text ?? "Unknown Source"
            }).ToList();

            return new IncomeIndexViewModel
            {
                Incomes = mappedIncomes,
                PaginatedItems = paginatedList,
                SelectedMonth = month,
                SelectedYear = year,
                SelectedSource = source,
                ShowAll = showAll,
                Months = _monthsHelper.GetMonths(),
                Sources = sourcesList,
                PageSize = pageSize
            };
        }
    }
}
