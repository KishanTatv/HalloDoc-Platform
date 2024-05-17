using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.Entity.Models;

[Table("cheatmessage")]
public partial class Cheatmessage
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("sender")]
    public int Sender { get; set; }

    [Column("reciver")]
    public int Reciver { get; set; }

    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("messages", TypeName = "character varying")]
    public string? Messages { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime? Createddate { get; set; }
}
