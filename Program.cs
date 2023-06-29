using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Expense_Tracker.Data;
var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<Expense_TrackerContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("Expense_TrackerContext") ?? throw new InvalidOperationException("Connection string 'Expense_TrackerContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<Expense_TrackerContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ExpenseTrackerConnectionString")));

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2V1hhQlJCfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5VdE1iXHpYc3ZSQ2da");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
