using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using ExpenseTracker.Services.Interface;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly HelperMethods _helperMethods;
        private readonly IExpenseService _expenseService;

        public ExpenseController(HelperMethods helperMethods, IExpenseService expensesService)
        {
            _helperMethods = helperMethods;
            _expenseService = expensesService;
        }

        public async Task<IActionResult> Index(int? month, int? year, string? category, int pageNumber = 1, int pageSize = 10, bool showAll = false)
        {
            var expensesViewModel = await _expenseService.GetExpensesAsync(User, month, year, category, pageNumber, pageSize, showAll);
            return View(expensesViewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new ExpenseCreateEditViewModel
            {
                Categories = _helperMethods.GetCategories()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCreateEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _helperMethods.GetCategories();
                return View(viewModel);
            }

            var result = await _expenseService.CreateExpenseAsync(viewModel, User);
            if (result)
            {
                return RedirectToAction("Index", "Expense");
            }

            ModelState.AddModelError(string.Empty, "Unable to create expense. Please try again.");
            viewModel.Categories = _helperMethods.GetCategories();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _expenseService.GetExpenseByIdAsync(id, User);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _expenseService.EditExpenseAsync(model, User);

                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }

                return NotFound();
            }

            model.Categories = _helperMethods.GetCategories();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id, User);

            if (result)
            {
                return RedirectToAction("Index", "Expense");
            }

            return NotFound();
        }
    }
}
