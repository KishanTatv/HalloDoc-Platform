using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("providerfullsheet")]
public partial class Providerfullsheet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("peroid")]
    public short? Peroid { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("finalize")]
    public bool? Finalize { get; set; }

    [Column("startdate")]
    public DateOnly? Startdate { get; set; }

    [Column("enddate")]
    public DateOnly? Enddate { get; set; }

    [Column("bonus")]
    public int? Bonus { get; set; }

    [Column("description", TypeName = "character varying")]
    public string? Description { get; set; }

    [Column("approve")]
    public bool? Approve { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Providerfullsheets")]
    public virtual Physician Physician { get; set; } = null!;

    [InverseProperty("Sheet")]
    public virtual ICollection<Providerweeklysheet> Providerweeklysheets { get; } = new List<Providerweeklysheet>();
}
