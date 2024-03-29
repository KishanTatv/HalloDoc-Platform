using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.Entity.Models;

namespace HalloDoc.Entity.AdminTab
{
    public class adminViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public int? roleId { get; set; }
        public int? regionId { get; set; }


        [Column("firstname")]
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(15, MinimumLength = 3)]
        public string? Firstname { get; set; }


        [Column("lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(15, MinimumLength = 3)]
        public string? Lastname { get; set; }

        [Column("phonenumber")]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Column("Altphonenumber")]
        [StringLength(23)]
        public string? AltPhonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Column("ConfirmEmail")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please Enter Confirm Email")]
        [Compare("Email", ErrorMessage = "The Email and confirm Email do not match.")]
        [NotMapped]
        public string? ConfirmEmail { get; set; }

        [Column("street")]
        [StringLength(100)]
        public string? Street { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only String Enter.")]
        [StringLength(20, MinimumLength = 2)]
        public string? State { get; set; }

        [Column("zipcode")]
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"0*[1-9][0-9]*", ErrorMessage = "only number enter.")]
        public string? Zipcode { get; set; }

        [Column("Address")]
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }




        public List<Role> Roles { get; set; }

        public List<Region> Regions { get; set; }

        public List<Adminregion> AdminRegions { get; set; }

    }
}
