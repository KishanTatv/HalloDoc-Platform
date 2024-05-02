using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("providerpayrate")]
public partial class Providerpayrate
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("nightshift_weekend")]
    public int? NightshiftWeekend { get; set; }

    [Column("shift")]
    public int? Shift { get; set; }

    [Column("housecall_night_weekend")]
    public int? HousecallNightWeekend { get; set; }

    [Column("phoneconsults")]
    public int? Phoneconsults { get; set; }

    [Column("phoneconsults_night_weekend")]
    public int? PhoneconsultsNightWeekend { get; set; }

    [Column("batchtesting")]
    public int? Batchtesting { get; set; }

    [Column("housecalls")]
    public int? Housecalls { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Providerpayrates")]
    public virtual Physician Physician { get; set; } = null!;
}
