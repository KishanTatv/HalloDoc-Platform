using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entity.RequestForm
{
    public class ClientInformation
    {
        [Column("symptoms")]
        [StringLength(200)]
        public string? Symptoms { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string? Firstname { get; set; }

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("Dob")]
        public DateOnly? Dob { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        [Column("street")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [StringLength(100)]
        public string? State { get; set; }

        [Column("zipcode")]
        [StringLength(10)]
        public string? Zipcode { get; set; }

        [Column("Locroom")]
        [StringLength(100)]
        public string? Locroom { get; set; }

        [Column("relation")]
        [StringLength(100)]
        public string? relation { get; set; }

        // This property indicates whether the email exists in the database or not
        public bool EmailExists { get; set; } = false;
    }
}
