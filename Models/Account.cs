using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProject.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required, StringLength(20)]
        public string AccountNumber { get; set; }

        [Required, StringLength(100)]
        public string AccountHolderName { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        // Navigation
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public string? Email { get; internal set; }
        public string? PhoneNumber { get; internal set; }
        public string PasswordHash { get; internal set; }
    }

    public enum AccountType
    {
        Savings = 1,
        Current = 2,
        FixedDeposit = 3,
        Salary = 4
    }
}
