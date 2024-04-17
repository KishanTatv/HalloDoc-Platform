using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class AdminCustom
    {
        [Column("firstname")]
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string Firstname { get; set; } = null!;

        [Column("lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string? Lastname { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Please Enter Email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Column("mobile")]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(16, ErrorMessage = "Must be between 10 and 15 digit", MinimumLength = 9)]
        public string? Mobile { get; set; }

    }
}
