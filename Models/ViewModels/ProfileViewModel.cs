using System.ComponentModel.DataAnnotations;

namespace BankProject.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Full Name")]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Account Type")]
        public AccountType AccountType { get; set; }

        [Display(Name = "Account Created")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Current Balance")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Display(Name = "Address Line 1")]
        [StringLength(200)]
        public string? AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        [StringLength(200)]
        public string? AddressLine2 { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? State { get; set; }

        [Display(Name = "Zip/Postal Code")]
        [StringLength(20)]
        public string? PostalCode { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
