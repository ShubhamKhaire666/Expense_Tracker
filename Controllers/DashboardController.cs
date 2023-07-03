﻿using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Syncfusion.EJ2.Charts;
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

			//donut chart-expense by category
			ViewBag.DonutChartData = SelectedTransactions
				.Where(i => i.Category.Type == "Expense")
				.GroupBy(j => j.Category.CategoryId)
				.Select(k => new
				{
					categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
					amount = k.Sum(j => j.Amount),
					formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
				})
				.OrderByDescending(k=>k.amount)
				.ToList();


			//Spline Chart Income vs Expense
			List<SplineChartData> IncomeSummary = SelectedTransactions
				.Where(i => i.Category.Type == "Income")
				.GroupBy(j => j.Date)
				.Select(k => new SplineChartData()
				{
					day = k.First().Date.ToString("dd-MMM"),
					income = k.Sum(l => l.Amount)
				})
				.ToList();
			
			List<SplineChartData> ExpenseSummary = SelectedTransactions
				.Where(i => i.Category.Type == "Expense")
				.GroupBy(j => j.Date)
				.Select(k => new SplineChartData()
				{
					day = k.First().Date.ToString("dd-MMM"),
					expense = k.Sum(l => l.Amount)
				})
				.ToList();
			//combine income and expense
			string[] last7Days = Enumerable.Range(0, 7)
				.Select(i => StartDate.AddDays(i).ToString("dd-MMM"))
				.ToArray();

			ViewBag.SplineChartData = from day in last7Days
									  join income in IncomeSummary on day equals income.day into dayIncomeJoined
									  from income in dayIncomeJoined.DefaultIfEmpty()
									  join expense in ExpenseSummary on day equals expense.day into expenseJoined
									  from expense in expenseJoined.DefaultIfEmpty()
									  select new
									  {
										  day = day,
										  income = income == null ? 0 : income.income,
										  expense = expense == null ? 0 : expense.expense,
									  };


			//Recent Transaction
			ViewBag.RecentTransaction = await _context.Transactions
				.Include(j => j.Category)
				.OrderByDescending(j => j.Date)
				.Take(5)
				.ToListAsync();
			
			return View();
		}
	}

	public class SplineChartData
	{
		public string day;
		public int income;
		public int expense;
	}
}
