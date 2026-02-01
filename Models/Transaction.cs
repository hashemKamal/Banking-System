using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankProject.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required, StringLength(50)]
        public string TransactionReference { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int AccountId { get; set; }
        public int? ToAccountId { get; set; }

        // Navigation
        public Account Account { get; set; }
        public Account? ToAccount { get; set; }
    }

    public enum TransactionType
    {
        Deposit = 1,
        Withdrawal = 2,
        Transfer = 3,
        Interest = 4
    }
}
