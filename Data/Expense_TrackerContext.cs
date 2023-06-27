using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Data
{
    public class Expense_TrackerContext : DbContext
    {
        public Expense_TrackerContext (DbContextOptions<Expense_TrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Expense_Tracker.Models.Category> Category { get; set; } = default!;
    }
}
