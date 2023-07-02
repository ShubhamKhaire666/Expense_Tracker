using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Expense_Tracker.Controllers
{
	public class DashboardController : Controller
	{
		private readonly Expense_TrackerContext _context;

		public DashboardController(Expense_TrackerContext context)
        {
			_context = context;
		}
        public async Task<ActionResult> Index()
		{
			//Last 7 days
			DateTime StartDate = DateTime.Today.AddDays(-6);
			DateTime EndDate = DateTime.Today;

			List<Transaction> SelectedTransactions = await _context.Transactions
				.Include(x => x.Category)
				.Where(y => y.Date >= StartDate && y.Date <= EndDate)
				.ToListAsync();
			//Total Income
			int TotalIncome = SelectedTransactions
				.Where(i => i.Category.Type == "Income")
				.Sum(i => i.Amount);

			ViewBag.TotalIncome = TotalIncome.ToString("C0");

			//Total Expense
			int TotalExpense = SelectedTransactions
				.Where(i => i.Category.Type == "Expense")
				.Sum(i => i.Amount);

			ViewBag.TotalExpense = TotalExpense.ToString("C0");

			//Balance
			int Balance = TotalIncome- TotalExpense;
			CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
			culture.NumberFormat.CurrencyNegativePattern = 1;
			ViewBag.Balance = string.Format(culture,"{0:C0}",Balance);

			return View();
		}
	}
}
