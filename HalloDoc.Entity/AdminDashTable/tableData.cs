using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDashTable
{
    public class tableData
    {
        [Column("ReqId")]
        public int? RequestId { get; set; }

        [Column("ReqclientId")]
        public int? ReqClientId { get; set; }

        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Column("Age")]
        public int? Age { get; set; }

        [Column("strmonth", TypeName = "character varying")]
        public string? Strmonth { get; set; }

        [Column("intdate")]
        public int? Intdate { get; set; }

        [Column("intyear")]
        public int? Intyear { get; set; }

        [Column("ReqTypeId")]
        public int? ReqTypeId { get; set; }

        [Column("Requestor")]
        [StringLength(70)]
        public string? Requestor { get; set; }

        [Column("PhysicianName")]
        [StringLength(100)]
        public string? PhysicianName { get; set; }

        [Column("DateOfService")]
        public DateTime? DateOfService { get; set; }

        [Column("Region")]
        [StringLength(50)]
        public string? Region { get; set; }

        [Column("Request Date")]
        public DateTime? RequestedDate { get; set; }

        [Column("phonenumber")]
        [StringLength(23)]
        public string? Phonenumber { get; set; }

        [Column("reqnumber")]
        [StringLength(23)]
        public string? ReqPhonenumber { get; set; }

        [Column("Address")]
        [StringLength(100)]
        public string? Address { get; set; }

        [Column("city")]
        [StringLength(50)]
        public string? city { get; set; }

        [Column("Notegroup")]
        [StringLength(100)]
        public List<string> Notegroup { get; set; }


        [Column("Notes")]
        [StringLength(100)]
        public string? Notes { get; set; }

        [Column("ChatWith")]
        [StringLength(50)]
        public string? ChatWith { get; set; }


    }
}
