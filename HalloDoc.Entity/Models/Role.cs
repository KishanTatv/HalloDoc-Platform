﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("role")]
public partial class Role
{
    [Key]
    [Column("roleid")]
    public int Roleid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("accounttype")]
    public short Accounttype { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray Isdeleted { get; set; } = null!;

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("createdby")]
    public int? Createdby { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<Admin> Admins { get; } = new List<Admin>();

    [InverseProperty("Role")]
    public virtual ICollection<Physician> Physicians { get; } = new List<Physician>();

    [InverseProperty("Role")]
    public virtual ICollection<Rolemenu> Rolemenus { get; } = new List<Rolemenu>();
}
