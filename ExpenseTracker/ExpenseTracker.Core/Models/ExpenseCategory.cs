using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

    }
}
