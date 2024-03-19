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
        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; } = null!;

        [Column("lastname")]
        [StringLength(100)]
        public string? Lastname { get; set; }

        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;

        [Column("mobile")]
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
