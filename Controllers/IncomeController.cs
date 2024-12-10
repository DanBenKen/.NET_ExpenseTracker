using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels.IncomeViewModels;
using ExpenseTracker.Services.Interface;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SourcesHelper _sourcesHelper;
        private readonly IIncomeService _incomeService;

        public IncomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IIncomeService incomeService , SourcesHelper helperMethods)
        {
            _userManager = userManager;
            _sourcesHelper = helperMethods;
            _incomeService = incomeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? month, int? year, string? source, int pageNumber = 1, int pageSize = 5, bool showAll = false)
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = await _incomeService.GetIncomesViewModelAsync(userId, month, year, source, pageNumber, pageSize, showAll);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = _incomeService.GetCreateIncomeViewModel(userId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncomeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Sources = _sourcesHelper.GetSources();
                return View(viewModel);
            }

            var userId = _userManager.GetUserId(User);

            var success = await _incomeService.CreateIncomeAsync(viewModel, userId);
            if (!success)
            {
                viewModel.Sources = _sourcesHelper.GetSources();
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var viewModel = await _incomeService.GetIncomeForEditAsync(id, userId);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IncomeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var userId = _userManager.GetUserId(User);

            var isUpdated = await _incomeService.UpdateIncomeAsync(id, userId, viewModel);
            if (!isUpdated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var isDeleted = await _incomeService.DeleteIncomeAsync(id, userId);
            if (!isDeleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
