using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.ViewModels
{
    public class DepositViewModel
    {
        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Deposit Amount")]
        [Range(1, 50000, ErrorMessage = "Amount must be between $1 and $50,000")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Deposit method is required")]
        [Display(Name = "Deposit Method")]
        public DepositMethod DepositMethod { get; set; }

        [Display(Name = "Description (Optional)")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Display(Name = "Reference Number (If any)")]
        [StringLength(50, ErrorMessage = "Reference number cannot exceed 50 characters")]
        public string? ReferenceNumber { get; set; }
    }

    public enum DepositMethod
    {
        Cash,
        Check,
        WireTransfer,
        MobileDeposit,
        ATM
    }
}
