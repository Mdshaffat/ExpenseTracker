using ExpenseTracker.Core.Models;
using ExpenseTracker.DataAccess.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ExpenseTracker.DataAccess.Repository.IRepository
{
    public interface IExpensesRepository
    {
        Task<PaginatedList<Expenses>> ExpensesListAsync(DateTime? fromDate,
                                                        DateTime? toDate,
                                                        int? pageNumber);
        Task<Expenses> ExpenseByIdAsync(int id);
    }
}
