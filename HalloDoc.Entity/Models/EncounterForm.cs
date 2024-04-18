using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("EncounterForm")]
public partial class EncounterForm
{
    [Key]
    public int EncounterFormId { get; set; }

    public int? RequestId { get; set; }

    [Required(ErrorMessage = "Injury is required")]
    public string? HistoryOfPresentIllnessOrInjury { get; set; }

    [Required(ErrorMessage = "MedicalHistory is required")]
    public string? MedicalHistory { get; set; }

    [Required(ErrorMessage = "Medications is required")]
    public string? Medications { get; set; }

    [Required(ErrorMessage = "Allergies is required")]
    public string? Allergies { get; set; }

    [Required(ErrorMessage = "Temprature is required")]
    public string? Temp { get; set; }

    [Column("HR")]
    [Required(ErrorMessage = "HR is required")]
    public string? Hr { get; set; }

    [Column("RR")]
    [Required(ErrorMessage = "RR is required")]
    public string? Rr { get; set; }

    public string? BloodPressureSystolic { get; set; }

    public string? BloodPressureDiastolic { get; set; }

    public string? O2 { get; set; }

    public string? Pain { get; set; }

    public string? Heent { get; set; }

    [Column("CV")]
    public string? Cv { get; set; }

    [Required(ErrorMessage = "Chest is required")]
    public string? Chest { get; set; }

    [Column("ABD")]
    [Required(ErrorMessage = "ABD is required")]
    public string? Abd { get; set; }

    public string? Extremeties { get; set; }

    public string? Skin { get; set; }

    public string? Neuro { get; set; }

    public string? Other { get; set; }

    public string? Diagnosis { get; set; }

    public string? TreatmentPlan { get; set; }

    public string? MedicationsDispensed { get; set; }

    public string? Procedures { get; set; }

    public string? FollowUp { get; set; }

    public int? AdminId { get; set; }

    public int? PhysicianId { get; set; }

    public bool IsFinalize { get; set; }

    [ForeignKey("AdminId")]
    [InverseProperty("EncounterForms")]
    public virtual Admin? Admin { get; set; }

    [ForeignKey("PhysicianId")]
    [InverseProperty("EncounterForms")]
    public virtual Physician? Physician { get; set; }

    [ForeignKey("RequestId")]
    [InverseProperty("EncounterForms")]
    public virtual Request? Request { get; set; }
}
