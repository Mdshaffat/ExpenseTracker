using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Core.Models;
using ExpenseTracker.DataAccess.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpenseCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExpenseCategory
        public async Task<IActionResult> Index()
        {
              return View(await _context.ExpenseCategory.ToListAsync());
        }

        // GET: ExpenseCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExpenseCategory == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExpenseCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title")] ExpenseCategory expenseCategory)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    // Restrict Duplicate (1 st Way)
                    var expense = await _context.ExpenseCategory.FirstOrDefaultAsync(e => e.Title.ToLower() == expenseCategory.Title.ToLower());
                    if(expense != null)
                    {
                        ViewData["Exception"] = "Duplicate Entry";
                        return View(expenseCategory);
                    }
                    _context.Add(expenseCategory);
                    await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    // Restrict Duplicate (another Way)
                    if (ex.HResult == -2146233088)
                    {

                        ViewData["Exception"] = "Duplicate Entry";
                    }
                }
            }
            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExpenseCategory == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategory.FindAsync(id);
            if (expenseCategory == null)
            {
                return NotFound();
            }
            return View(expenseCategory);
        }

        // POST: ExpenseCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] ExpenseCategory expenseCategory)
        {
            if (id != expenseCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseCategoryExists(expenseCategory.Id))
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
            return View(expenseCategory);
        }

        // GET: ExpenseCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExpenseCategory == null)
            {
                return NotFound();
            }

            var expenseCategory = await _context.ExpenseCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenseCategory == null)
            {
                return NotFound();
            }

            return View(expenseCategory);
        }

        // POST: ExpenseCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExpenseCategory == null)
            {
                return Problem("Entity set 'ExpenseTrackerContext.ExpenseCategory'  is null.");
            }
            var expenseCategory = await _context.ExpenseCategory.FindAsync(id);
            if (expenseCategory != null)
            {
                _context.ExpenseCategory.Remove(expenseCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseCategoryExists(int id)
        {
          return _context.ExpenseCategory.Any(e => e.Id == id);
        }
    }
}
