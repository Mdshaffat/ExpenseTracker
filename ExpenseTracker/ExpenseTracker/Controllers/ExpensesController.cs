using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Core.Models;
using ExpenseTracker.DataAccess.Data;
using NuGet.Packaging.Core;
using ExpenseTracker.DataAccess.Paging;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.DataAccess.Repository.IRepository;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        // Implemented Repository Patern
        private readonly IExpensesRepository expensesRepository;

        public ExpensesController(ApplicationDbContext context, IExpensesRepository expensesRepository)
        {
            _context = context;
            this.expensesRepository = expensesRepository;
        }

        // GET: Expenses
        public async Task<IActionResult> Index(DateTime? fromDate,
                                               DateTime? toDate, 
                                               DateTime? currentToDate, 
                                               DateTime? currentFromDate,
                                               int? pageNumber)
        {
            
            if (fromDate != null && toDate != null)
            {
                pageNumber = 1;
            }
            else
            {
                fromDate = currentFromDate;
                toDate = currentToDate;
            }
            ViewData["CurrentFromDate"] = fromDate;
            ViewData["CurrentToDate"] = toDate;
            // Implemented Repository Patern
            PaginatedList<Expenses> expenseTracker = await expensesRepository.ExpensesListAsync(fromDate,toDate,pageNumber);
            return View(expenseTracker);
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var expenses = await expensesRepository.ExpenseByIdAsync(id);

            return View(expenses);
        }

        // GET: Expenses/Expenses
        public IActionResult Expenses()
        {
            ViewData["ExpenseCategoryId"] = new SelectList(_context.Set<ExpenseCategory>(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Expenses([Bind("Id,ExpenseCategoryId,DateOfTheExpense,ExpenseAmount")] Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expenses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseCategoryId"] = new SelectList(_context.Set<ExpenseCategory>(), "Id", "Title", expenses.ExpenseCategoryId);
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expense == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expense.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }
            ViewData["ExpenseCategoryId"] = new SelectList(_context.Set<ExpenseCategory>(), "Id", "Title", expenses.ExpenseCategoryId);
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExpenseCategoryId,DateOfTheExpense,ExpenseAmount")] Expenses expenses)
        {
            if (id != expenses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensesExists(expenses.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpenseCategoryId"] = new SelectList(_context.Set<ExpenseCategory>(), "Id", "Title", expenses.ExpenseCategoryId);
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expense == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expense
                .Include(e => e.ExpenseCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expense == null)
            {
                return Problem("Entity set 'ExpenseTrackerContext.Expenses'  is null.");
            }
            var expenses = await _context.Expense.FindAsync(id);
            if (expenses != null)
            {
                _context.Expense.Remove(expenses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
          return _context.Expense.Any(e => e.Id == id);
        }
    }
}
