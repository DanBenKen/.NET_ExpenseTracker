﻿using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Models.ViewModels;
using ExpenseTracker.Models.ViewModels.ExpenseViewModels;
using ExpenseTracker.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly HelperMethods _helperMethods;

        public ExpenseController(ApplicationDbContext context, UserManager<User> userManager, HelperMethods helperMethods)
        {
            _context = context;
            _userManager = userManager;
            _helperMethods = helperMethods;
        }

        public async Task<IActionResult> Index(int? month, int? year, string category)
        {
            var userId = _userManager.GetUserId(User);

            var expensesQuery = _context.Expenses
                .Where(e => e.UserId == userId);

            bool hasValidCategory = !string.IsNullOrEmpty(category) && category != "AllCategories";
            month ??= DateTime.Now.Month;
            year ??= DateTime.Now.Year;

            if (hasValidCategory)
            {
                expensesQuery = expensesQuery.Where(e => e.Category == category);
            }

            expensesQuery = expensesQuery.Where(e => e.Date.Month == month && e.Date.Year == year);

            var expenses = await expensesQuery.ToListAsync();

            var viewModel = new ExpenseIndexViewModel
            {
                Expenses = expenses,
                SelectedMonth = month,
                SelectedYear = year,
                SelectedCategory = category,
                Months = _helperMethods.GetMonths(),
                Categories = _helperMethods.GetCategories()
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            var expense = _context.Expenses.FirstOrDefault(e => e.UserId == userId);

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

            var userId = _userManager.GetUserId(User);
            var expense = new Expense
            {
                Description = viewModel.Description,
                Amount = viewModel.Amount,
                Date = viewModel.Date,
                Category = viewModel.Category,
                UserId = userId
            };

            _context.Add(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            var viewModel = new ExpenseCreateEditViewModel
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = expense.Date,
                Description = expense.Description,
                Categories = _helperMethods.GetCategories()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExpenseCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == model.Id && e.UserId == userId);

                if (expense == null)
                {
                    return NotFound();
                }

                expense.Date = model.Date;
                expense.Description = model.Description;
                expense.Category = model.Category;
                expense.Amount = model.Amount;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            model.Categories = _helperMethods.GetCategories();

            return View(model);
        }
    }
}