using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entity.RequestForm
{
    public class FormFCB
    {

        public ClientInformation clientInformation { get; set; }

        [Column("patientFname")]
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(100)]
        public string? PatientFname { get; set; }

        [Column("patientLname")]
        [StringLength(100)]
        public string? PatientLname { get; set; }

        [Column("patientPhonenumber")]
        [StringLength(23)]
        public string? PatientPhonenumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Column("patientEmail")]
        [StringLength(50)]
        public string? PatientEmail { get; set; }

        [Column("patientProperty")]
        [StringLength(23)]
        public string? PatientProperty { get; set; }

        [Column("patientCase")]
        [StringLength(23)]
        public string? PatientCase { get; set; }


        [Column("patientStreet")]
        [StringLength(100)]
        public string? PatientStreet { get; set; }

        [Column("patientCity")]
        [StringLength(100)]
        public string? PatientCity { get; set; }

        [Column("patientState")]
        [StringLength(100)]
        public string? PatientState { get; set; }

        [Column("patientZipcode")]
        [StringLength(10)]
        public string? PatientZipcode { get; set; }

    }
}
