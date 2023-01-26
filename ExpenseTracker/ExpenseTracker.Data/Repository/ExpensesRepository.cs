using ExpenseTracker.Core.Models;
using ExpenseTracker.DataAccess.Data;
using ExpenseTracker.DataAccess.Paging;
using ExpenseTracker.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExpenseTracker.DataAccess.Repository
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly ApplicationDbContext context;

        public ExpensesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }


        public async Task<PaginatedList<Expenses>> ExpensesListAsync(DateTime? fromDate,
                                                                     DateTime? toDate,
                                                                     int? pageNumber)
        {
            int pageSize = 10;
            IQueryable<Expenses> expenseTrackerContext = context.Expense.Include(e => e.ExpenseCategory);
            if (fromDate != null && toDate != null)
            {
                expenseTrackerContext = expenseTrackerContext.Where(s => s.DateOfTheExpense.Date >= fromDate.Value.Date && s.DateOfTheExpense.Date <= toDate.Value.Date);
            }
            return await PaginatedList<Expenses>.CreateAsync(expenseTrackerContext.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
        public async Task<Expenses> ExpenseByIdAsync(int id)
        {
            var expenses = await context.Expense
                .Include(e => e.ExpenseCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            return expenses;
        }

    }
}
