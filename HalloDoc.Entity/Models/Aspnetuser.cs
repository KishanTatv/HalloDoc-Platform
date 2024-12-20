﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("aspnetusers")]
public partial class Aspnetuser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(256)]
    public string? Username { get; set; }

    [Column("passwordhash", TypeName = "character varying")]
    public string? Passwordhash { get; set; }

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("phonenumber")]
    [StringLength(20)]
    public string? Phonenumber { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Admin> AdminAspnetusers { get; } = new List<Admin>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Admin> AdminModifiedbyNavigations { get; } = new List<Admin>();

    [InverseProperty("User")]
    public virtual Aspnetuserrole? Aspnetuserrole { get; set; }

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Business> BusinessCreatedbyNavigations { get; } = new List<Business>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Business> BusinessModifiedbyNavigations { get; } = new List<Business>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<Physician> PhysicianAspnetusers { get; } = new List<Physician>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Physician> PhysicianCreatedbyNavigations { get; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Physician> PhysicianModifiedbyNavigations { get; } = new List<Physician>();

    [InverseProperty("ModifiedbyNavigation")]
    public virtual ICollection<Shiftdetail> Shiftdetails { get; } = new List<Shiftdetail>();

    [InverseProperty("CreatedbyNavigation")]
    public virtual ICollection<Shift> Shifts { get; } = new List<Shift>();

    [InverseProperty("Aspnetuser")]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
