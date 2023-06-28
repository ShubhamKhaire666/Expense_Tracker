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
        public Expense_TrackerContext (DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
