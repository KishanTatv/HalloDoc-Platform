using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalloDoc.Entity.RequestForm
{
    public class RequestWithFile
    {
        [Column("requestwisefileid")]
        public int Requestwisefileid { get; set; }

        [Column("requestid")]
        public int Requestid { get; set; }

        [Column("firstname")]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Column("createddate", TypeName = "timestamp without time zone")]
        public DateTime Createddate { get; set; }

        [Column("status")]
        public short Status { get; set; }

        [Column("filename")]
        [StringLength(500)]
        public string Filename { get; set; }

        [Column("UploadDate", TypeName = "timestamp without time zone")]
        public DateTime UploadDate { get; set; }

        [Column("fileCount")]
        public int FileCount { get; set; }


    }
}
