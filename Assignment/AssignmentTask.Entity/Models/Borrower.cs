using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AssignmentTask.Entity.Models
{
    [Table("borrower")]
    public partial class Borrower
    {
        public Borrower()
        {
            Books = new HashSet<Book>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("city")]
        [StringLength(256)]
        public string City { get; set; } = null!;

        [InverseProperty("Borrower")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
