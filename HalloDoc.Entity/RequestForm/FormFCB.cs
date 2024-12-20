﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HalloDoc.Entity.RequestForm
{
    public class FormFCB
    {

        public ClientInformation clientInformation { get; set; }

        [Column("patientFname")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 20 characters", MinimumLength = 2)]
        public string? PatientFname { get; set; }

        [Column("patientLname")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Characters Enter.")]
        [Required(ErrorMessage = "LastName is required")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string? PatientLname { get; set; }

        [Column("patientPhonenumber")]
        [Required(ErrorMessage = "PhoneNumber is required")]
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
