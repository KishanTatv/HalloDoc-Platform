﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("physician")]
public partial class Physician
{
    [Key]
    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("aspnetuserid")]
    public int? Aspnetuserid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string Email { get; set; } = null!;

    [Column("mobile")]
    [StringLength(20)]
    public string? Mobile { get; set; }

    [Column("medicallicense")]
    [StringLength(500)]
    public string? Medicallicense { get; set; }

    [Column("photo", TypeName = "character varying")]
    public string? Photo { get; set; }

    [Column("adminnotes")]
    [StringLength(500)]
    public string? Adminnotes { get; set; }

    [Column("isagreementdoc", TypeName = "bit(1)")]
    public BitArray? Isagreementdoc { get; set; }

    [Column("isbackgrounddoc", TypeName = "bit(1)")]
    public BitArray? Isbackgrounddoc { get; set; }

    [Column("istrainingdoc", TypeName = "bit(1)")]
    public BitArray? Istrainingdoc { get; set; }

    [Column("isnondisclosuredoc", TypeName = "bit(1)")]
    public BitArray? Isnondisclosuredoc { get; set; }

    [Column("address1")]
    [StringLength(500)]
    public string? Address1 { get; set; }

    [Column("address2")]
    [StringLength(500)]
    public string? Address2 { get; set; }

    [Column("city")]
    [StringLength(100)]
    public string? City { get; set; }

    [Column("regionid")]
    public int? Regionid { get; set; }

    [Column("zip")]
    [StringLength(10)]
    public string? Zip { get; set; }

    [Column("altphone")]
    [StringLength(20)]
    public string? Altphone { get; set; }

    [Column("createdby")]
    public int Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("status")]
    public short? Status { get; set; }

    [Column("businessname")]
    [StringLength(100)]
    public string Businessname { get; set; } = null!;

    [Column("businesswebsite")]
    [StringLength(200)]
    public string Businesswebsite { get; set; } = null!;

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    [Column("roleid")]
    public int? Roleid { get; set; }

    [Column("npinumber")]
    [StringLength(500)]
    public string? Npinumber { get; set; }

    [Column("islicensedoc", TypeName = "bit(1)")]
    public BitArray? Islicensedoc { get; set; }

    [Column("signature", TypeName = "character varying")]
    public string? Signature { get; set; }

    [Column("iscredentialdoc", TypeName = "bit(1)")]
    public BitArray? Iscredentialdoc { get; set; }

    [Column("istokengenerate", TypeName = "bit(1)")]
    public BitArray? Istokengenerate { get; set; }

    [Column("syncemailaddress")]
    [StringLength(50)]
    public string? Syncemailaddress { get; set; }

    [ForeignKey("Aspnetuserid")]
    [InverseProperty("PhysicianAspnetusers")]
    public virtual Aspnetuser? Aspnetuser { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("PhysicianCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [InverseProperty("Physician")]
    public virtual ICollection<EncounterForm> EncounterForms { get; } = new List<EncounterForm>();

    [ForeignKey("Modifiedby")]
    [InverseProperty("PhysicianModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianlocation> Physicianlocations { get; } = new List<Physicianlocation>();

    [InverseProperty("Physician")]
    public virtual ICollection<Physiciannotification> Physiciannotifications { get; } = new List<Physiciannotification>();

    [InverseProperty("Physician")]
    public virtual ICollection<Physicianregion> Physicianregions { get; } = new List<Physicianregion>();

    [InverseProperty("Physician")]
    public virtual ICollection<Providerfullsheet> Providerfullsheets { get; } = new List<Providerfullsheet>();

    [InverseProperty("Physician")]
    public virtual ICollection<Providerpayrate> Providerpayrates { get; } = new List<Providerpayrate>();

    [InverseProperty("Physician")]
    public virtual ICollection<Request> Requests { get; } = new List<Request>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogPhysicians { get; } = new List<Requeststatuslog>();

    [InverseProperty("Transtophysician")]
    public virtual ICollection<Requeststatuslog> RequeststatuslogTranstophysicians { get; } = new List<Requeststatuslog>();

    [InverseProperty("Physician")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; } = new List<Requestwisefile>();

    [ForeignKey("Roleid")]
    [InverseProperty("Physicians")]
    public virtual Role? Role { get; set; }

    [InverseProperty("Physician")]
    public virtual ICollection<Shift> Shifts { get; } = new List<Shift>();
}
