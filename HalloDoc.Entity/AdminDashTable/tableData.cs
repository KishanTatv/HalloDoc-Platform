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
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        [Column("Dob")]
        public DateOnly? Dob { get; set; }

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

        [Column("Address")]
        [StringLength(100)]
        public string? Address { get; set; }

        [Column("Notes")]
        [StringLength(100)]
        public string? Notes { get; set; }

        [Column("ChatWith")]
        [StringLength(50)]
        public string? ChatWith { get; set; }


    }
}
