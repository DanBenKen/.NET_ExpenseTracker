using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Utils;
using ExpenseTracker.Utils.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly CategoryHelper _categoryHelper;
        private readonly IExpenseService _expenseService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExpenseController(CategoryHelper categoryHelper, IExpenseService expensesService, UserManager<ApplicationUser> userManager)
        {
            _categoryHelper = categoryHelper;
            _expenseService = expensesService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? month, int? year, string? category, int pageNumber = 1, int pageSize = 5, bool showAll = false)
        {
            var userId = _userManager.GetUserId(User);

            var expensesViewModel = await _expenseService.GetExpensesAsync(userId, month, year, category, pageNumber, pageSize, showAll);
            return View(expensesViewModel);
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = _expenseService.GetCreateExpenseViewModel(userId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseCreateEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _categoryHelper.GetCategories();
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);

            var result = await _expenseService.CreateExpenseAsync(viewModel, userId);
            if (!result)
            {
                viewModel.Categories = _categoryHelper.GetCategories();
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = await _expenseService.GetExpenseByIdAsync(id, userId);
            if (viewModel == null)
                return NotFound("Expense not found.");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseCreateEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoryHelper.GetCategories();
                return View(model);
            }

            var userId = _userManager.GetUserId(User);

            try
            {
                var result = await _expenseService.UpdateExpenseAsync(model, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (ExpenseNotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Categories = _categoryHelper.GetCategories();
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                model.Categories = _categoryHelper.GetCategories();
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var result = await _expenseService.DeleteExpenseAsync(id, userId);
            if (!result)
                return NotFound("Expense not found.");

            return RedirectToAction(nameof(Index));
        }
    }
}
