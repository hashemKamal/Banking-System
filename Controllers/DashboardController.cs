using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankProject.Data;
using BankProject.Models;
using System.Security.Claims;

namespace BankProject.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BankingContext _context;

        public DashboardController(BankingContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var accountClaim = User.FindFirst("AccountId")?.Value;
            if (string.IsNullOrEmpty(accountClaim) || !int.TryParse(accountClaim, out var accountId))
                return RedirectToAction("Login", "Account");

            var account = await _context.Accounts
                .Include(a => a.Transactions.OrderByDescending(t => t.TransactionDate).Take(50))
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null) return RedirectToAction("Login", "Account");

            return View(account);
        }
        public async Task<IActionResult> Profile()
        {
            var accountClaim = User.FindFirst("AccountId")?.Value;
            if (string.IsNullOrEmpty(accountClaim) || !int.TryParse(accountClaim, out var accountId))
                return RedirectToAction("Login", "Account");

            var account = await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null) return RedirectToAction("Login", "Account");

            return View(account);
        }
    }
}

    