using BankProject.Data;
using BankProject.Models;
using BankProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankProject.Controllers
{
    public class TransactionController : Controller
    {
        private readonly BankingContext _context;

        public TransactionController(BankingContext context)
        {
            _context = context;
        }

        // GET: Transaction/Index
        public async Task<IActionResult> Index()
        {
            var accountId = User.FindFirstValue("AccountId");
            if (string.IsNullOrEmpty(accountId))
                return RedirectToAction("Login", "Account");

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == int.Parse(accountId))
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View(transactions);
        }

        // GET: Transaction/Deposit
        public IActionResult Deposit() => View();

        // POST: Transaction/Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(decimal amount, string description)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater than zero.");
                return View();
            }

            var accountId = User.FindFirstValue("AccountId");
            var account = await _context.Accounts.FindAsync(int.Parse(accountId));

            if (account == null)
                return NotFound();

            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Amount = amount,
                TransactionType = TransactionType.Deposit,
                Description = description ?? "Cash Deposit",
                TransactionDate = DateTime.Now,
                TransactionReference = GenerateTransactionReference(),
                BalanceAfter = account.Balance + amount
            };

            account.Balance += amount;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully deposited ${amount}";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Transaction/Withdraw
        public IActionResult Withdraw() => View();

        // POST: Transaction/Withdraw
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(decimal amount, string description)
        {
            var accountId = User.FindFirstValue("AccountId");
            var account = await _context.Accounts.FindAsync(int.Parse(accountId));

            if (account == null)
                return NotFound();

            if (amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater than zero.");
                return View();
            }

            if (account.Balance < amount)
            {
                ModelState.AddModelError("", "Insufficient funds.");
                return View();
            }

            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Amount = amount,
                TransactionType = TransactionType.Withdrawal,
                Description = description ?? "Cash Withdrawal",
                TransactionDate = DateTime.Now,
                TransactionReference = GenerateTransactionReference(),
                BalanceAfter = account.Balance - amount
            };

            account.Balance -= amount;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Successfully withdrew ${amount}";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Transaction/Transfer
        public IActionResult Transfer()
        {
            return View(new TransferViewModel());
        }

        // POST: Transaction/Transfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            model.ToAccountNumber = model.ToAccountNumber?.Trim().ToUpper();

            if (!ModelState.IsValid)
                return View(model);

            var accountId = User.FindFirstValue("AccountId");
            if (string.IsNullOrEmpty(accountId))
                return RedirectToAction("Login", "Account");

            int fromAccountId = int.Parse(accountId);

            var fromAccount = await _context.Accounts.FindAsync(fromAccountId);
            var toAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == model.ToAccountNumber);

            if (fromAccount == null || toAccount == null)
            {
                ModelState.AddModelError("", "Account not found");
                return View(model);
            }

            if (fromAccount.AccountId == toAccount.AccountId)
            {
                ModelState.AddModelError("", "You cannot transfer to the same account");
                return View(model);
            }

            if (fromAccount.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Insufficient funds");
                return View(model);
            }

            // Generate unique references for each transaction
            var withdrawalReference = GenerateTransactionReference();
            var depositReference = GenerateTransactionReference();

            var withdrawal = new Transaction
            {
                AccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = model.Amount,
                TransactionType = TransactionType.Transfer,
                Description = model.Description ?? $"Transfer to {toAccount.AccountNumber}",
                TransactionDate = DateTime.Now,
                TransactionReference = withdrawalReference,
                BalanceAfter = fromAccount.Balance - model.Amount
            };

            var deposit = new Transaction
            {
                AccountId = toAccount.AccountId,
                ToAccountId = fromAccount.AccountId,
                Amount = model.Amount,
                TransactionType = TransactionType.Transfer,
                Description = model.Description ?? $"Transfer from {fromAccount.AccountNumber}",
                TransactionDate = DateTime.Now,
                TransactionReference = depositReference,
                BalanceAfter = toAccount.Balance + model.Amount
            };

            fromAccount.Balance -= model.Amount;
            toAccount.Balance += model.Amount;

            _context.Transactions.Add(withdrawal);
            _context.Transactions.Add(deposit);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Transfer completed successfully";
            return RedirectToAction("Index", "Dashboard");
        }

        // Generate unique TransactionReference
        private string GenerateTransactionReference()
        {
            return "TXN" + Guid.NewGuid().ToString("N"); 
        }
    }
}
