﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("request")]
public partial class Request
{
    [Key]
    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("requesttypeid")]
    public int Requesttypeid { get; set; }

    [Column("userid")]
    public int? Userid { get; set; }

    [Column("firstname")]
    [StringLength(100)]
    public string? Firstname { get; set; }

    [Column("lastname")]
    [StringLength(100)]
    public string? Lastname { get; set; }

    [Column("phonenumber")]
    [StringLength(23)]
    public string? Phonenumber { get; set; }

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("status")]
    public short Status { get; set; }

    [Column("physicianid")]
    public int? Physicianid { get; set; }

    [Column("confirmationnumber")]
    [StringLength(20)]
    public string? Confirmationnumber { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("isdeleted", TypeName = "bit(1)")]
    public BitArray? Isdeleted { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("declinedby")]
    [StringLength(250)]
    public string? Declinedby { get; set; }

    [Column("isurgentemailsent", TypeName = "bit(1)")]
    public BitArray? Isurgentemailsent { get; set; }

    [Column("lastwellnessdate", TypeName = "timestamp without time zone")]
    public DateTime? Lastwellnessdate { get; set; }

    [Column("ismobile", TypeName = "bit(1)")]
    public BitArray? Ismobile { get; set; }

    [Column("calltype")]
    public short? Calltype { get; set; }

    [Column("completedbyphysician", TypeName = "bit(1)")]
    public BitArray? Completedbyphysician { get; set; }

    [Column("lastreservationdate", TypeName = "timestamp without time zone")]
    public DateTime? Lastreservationdate { get; set; }

    [Column("accepteddate", TypeName = "timestamp without time zone")]
    public DateTime? Accepteddate { get; set; }

    [Column("relationname")]
    [StringLength(100)]
    public string? Relationname { get; set; }

    [Column("casenumber")]
    [StringLength(50)]
    public string? Casenumber { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("casetag")]
    [StringLength(50)]
    public string? Casetag { get; set; }

    [Column("casetagphysician")]
    [StringLength(50)]
    public string? Casetagphysician { get; set; }

    [Column("patientaccountid")]
    [StringLength(128)]
    public string? Patientaccountid { get; set; }

    [Column("createduserid")]
    public int? Createduserid { get; set; }

    [StringLength(200)]
    public string? Symptoms { get; set; }

    [Column("locroom")]
    [StringLength(100)]
    public string? Locroom { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Blockrequest> Blockrequests { get; } = new List<Blockrequest>();

    [InverseProperty("Request")]
    public virtual ICollection<Emaillog> Emaillogs { get; } = new List<Emaillog>();

    [InverseProperty("Request")]
    public virtual ICollection<EncounterForm> EncounterForms { get; } = new List<EncounterForm>();

    [ForeignKey("Physicianid")]
    [InverseProperty("Requests")]
    public virtual Physician? Physician { get; set; }

    [InverseProperty("Request")]
    public virtual ICollection<Requestbusiness> Requestbusinesses { get; } = new List<Requestbusiness>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestclient> Requestclients { get; } = new List<Requestclient>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestclosed> Requestcloseds { get; } = new List<Requestclosed>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestconcierge> Requestconcierges { get; } = new List<Requestconcierge>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestnote> Requestnotes { get; } = new List<Requestnote>();

    [InverseProperty("Request")]
    public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; } = new List<Requeststatuslog>();

    [InverseProperty("Request")]
    public virtual ICollection<Requestwisefile> Requestwisefiles { get; } = new List<Requestwisefile>();

    [InverseProperty("Request")]
    public virtual ICollection<Smslog> Smslogs { get; } = new List<Smslog>();

    [ForeignKey("Userid")]
    [InverseProperty("Requests")]
    public virtual User? User { get; set; }
}
