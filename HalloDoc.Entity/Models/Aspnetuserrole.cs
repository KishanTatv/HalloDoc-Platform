using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Key]
    [Column("userid")]
    public int Userid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetrole Role { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserrole")]
    public virtual Aspnetuser User { get; set; } = null!;
}
