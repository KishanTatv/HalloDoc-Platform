using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("providerweeklysheet")]
public partial class Providerweeklysheet
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("weekdate")]
    public DateOnly? Weekdate { get; set; }

    [Column("isholiday")]
    public bool? Isholiday { get; set; }

    [Column("totalhours")]
    public int? Totalhours { get; set; }

    [Column("housecall")]
    public int? Housecall { get; set; }

    [Column("consult")]
    public int? Consult { get; set; }

    [ForeignKey("Physicianid")]
    [InverseProperty("Providerweeklysheets")]
    public virtual Physician Physician { get; set; } = null!;
}
