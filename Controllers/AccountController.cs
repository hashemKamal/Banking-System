using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankProject.Models;
using BankProject.Models.ViewModels;
using BankProject.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace BankProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankingContext _context;
        private readonly PasswordHasher<Account> _hasher = new();

        public AccountController(BankingContext context)
        {
            _context = context;
        }
        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.Email == model.Email);

            if (account == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, account.AccountHolderName),
        new Claim(ClaimTypes.Email, account.Email),
        new Claim("AccountId", account.AccountId.ToString()),
        new Claim("AccountNumber", account.AccountNumber)
    };

            var identity = new ClaimsIdentity(claims, "BankCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("BankCookie", principal, new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            });

            return RedirectToAction("Index", "Dashboard");
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _context.Accounts.AnyAsync(a => a.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            var customer = new Customer
            {
                FullName = model.AccountHolderName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                NationalId = model.NationalId
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var account = new Account
            {
                CustomerId = customer.CustomerId,
                AccountHolderName = model.AccountHolderName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AccountType = model.AccountType,
                AccountNumber = GenerateAccountNumber(),
                CreatedDate = DateTime.Now,
                IsActive = true,
                Balance = model.InitialDeposit
            };

            account.PasswordHash = _hasher.HashPassword(account, model.Password);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            if (model.InitialDeposit > 0)
            {
                _context.Transactions.Add(new Transaction
                {
                    AccountId = account.AccountId,
                    Amount = model.InitialDeposit,
                    BalanceAfter = account.Balance,
                    Description = "Initial Deposit",
                    TransactionDate = DateTime.Now,
                    TransactionReference = GenerateTransactionReference(),
                    TransactionType = TransactionType.Deposit
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Login");
        }
        // GET: Account/Logout
        public IActionResult Logout()
        {
            return View();
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutConfirmed()
        {
            await HttpContext.SignOutAsync("BankCookie");
            return RedirectToAction("Login", "Account");
        }

        private string GenerateAccountNumber()
        {
            return "ACC" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }


        private string GenerateTransactionReference()
        {
            return "TXN" + DateTime.Now.Ticks;
        }

    }
}
