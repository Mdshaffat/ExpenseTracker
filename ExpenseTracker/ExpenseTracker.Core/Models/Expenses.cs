using ExpenseTracker.Core.ModelValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory? ExpenseCategory { get; set; }
        [DataType(DataType.Date)]
        [DateLessThanOrEqualToToday]
        public DateTime DateOfTheExpense { get; set; }
        [RegularExpression(@"-?\d+(?:\.\d+)?", ErrorMessage = "Please enter valid number")]
        public double ExpenseAmount { get; set; }
    }
}
