using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.ViewModels
{
    public class WithdrawViewModel
    {
        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Withdrawal Amount")]
        [Range(1, 10000, ErrorMessage = "Amount must be between $1 and $10,000")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Withdrawal method is required")]
        [Display(Name = "Withdrawal Method")]
        public WithdrawalMethod WithdrawalMethod { get; set; }

        [Display(Name = "Description (Optional)")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

    }

    public enum WithdrawalMethod
    {
        ATM,
        BankTeller,
        OnlineTransfer,
        Check
    }
}
