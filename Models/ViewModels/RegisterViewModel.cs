using System.ComponentModel.DataAnnotations;
using BankProject.Models;

namespace BankProject.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string? AccountHolderName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        [Display(Name = "National ID")]
        [StringLength(20, ErrorMessage = "National ID cannot exceed 20 characters")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]

        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select account type")]
        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{6,}$", ErrorMessage = "Password must have at least one letter and one number")]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]

        public string? ConfirmPassword { get; set; }

        [Display(Name = "Initial Deposit")]
        [Range(0, 10000, ErrorMessage = "Initial deposit must be between $0 and $10,000")]
        public decimal InitialDeposit { get; set; } = 0;

        [Required]
        [Display(Name = "Terms and Conditions")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms and conditions")]
        public bool AcceptTerms { get; set; }
    }
}
