using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class PhysicianCustom
    {
        [Column("physicianid")]
        public int Physicianid { get; set; }

        [Column("firstname")]
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(100)]
        public string Firstname { get; set; } = null!;

        [Column("lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Please Enter Email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Column("mobile")]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(20)]
        public string? Mobile { get; set; }

        [Column("medicallicense")]
        [StringLength(500)]
        public string? Medicallicense { get; set; }

        [Column("npinumber")]
        [StringLength(500)]
        public string? Npinumber { get; set; }

        [Column("syncemailaddress")]
        [StringLength(50)]
        public string? Syncemailaddress { get; set; }
    }
}
