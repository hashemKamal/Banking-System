using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.ViewModels
{
    public class TransferViewModel
    {
        [Required(ErrorMessage = "Recipient account number is required")]
        [Display(Name = "To Account Number")]
//        [RegularExpression(@"^ACC\d+$", ErrorMessage = "Account number must start with 'ACC' followed by numbers")]
        public string ToAccountNumber { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        [Range(1, 10000, ErrorMessage = "Amount must be between $1 and $10,000")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Display(Name = "Description (Optional)")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
    }
}
