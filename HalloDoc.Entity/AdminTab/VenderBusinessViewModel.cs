using HalloDoc.Entity.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class VenderBusinessViewModel
    {
        [Column("vendorid")]
        public int Vendorid { get; set; }

        [Column("vendorname")]
        [Required(ErrorMessage = "name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [StringLength(100, MinimumLength = 3)]
        public string Vendorname { get; set; } = null!;

        [Column("profession")]
        [Required(ErrorMessage = "Select Profession")]
        public int? Profession { get; set; }

        [Column("faxnumber")]
        [StringLength(50)]
        public string Faxnumber { get; set; } = null!;

        [Column("address")]
        [StringLength(150)]
        public string? Address { get; set; }

        [Column("city")]
        [StringLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only String Enter.")]
        [StringLength(50)]
        public string? State { get; set; }

        [Column("zip")]
        [Required(ErrorMessage = "ZipCode is required")]
        [RegularExpression(@"0*[1-9][0-9]*", ErrorMessage = "only number enter.")]
        [StringLength(50)]
        public string? Zip { get; set; }

        [Column("regionid")]
        public int? Regionid { get; set; }


        [Column("phonenumber")]
        [StringLength(100)]
        [Required(ErrorMessage = "phoneNumber is required")]
        [RegularExpression(@"0*[1-9][0-9]*", ErrorMessage = "only number enter.")]
        public string? Phonenumber { get; set; }

        [Column("email")]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Required(ErrorMessage = "Please Enter Email")]
        public string? Email { get; set; }

        [Column("businesscontact")]
        [StringLength(100)]
        [Required(ErrorMessage = "Business Contact is required")]
        [RegularExpression(@"0*[1-9][0-9]*", ErrorMessage = "only number enter.")]
        public string? Businesscontact { get; set; }


        public List<Healthprofessionaltype> Healthprofessionaltypes { get; set; }
    }
}
