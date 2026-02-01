using System.ComponentModel.DataAnnotations;

namespace BankProject.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; }

        [Phone, StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(14)]
        public string NationalId { get; set; }   

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
