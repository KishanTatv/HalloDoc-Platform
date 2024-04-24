using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentTask.Entity.ViewModels
{
    public class BookPopupViewModel
    {
        public int? id { get; set; }

        [Required(ErrorMessage = "BookName is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [StringLength(20, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Borrower Name is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string BorrowerName { get; set; }


        [Column("Dob")]
        [Required(ErrorMessage = "Select Issue Date")]
        public DateTime DateOfIssue { get; set; }

        [Required(ErrorMessage = "Genere is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string Genere { get; set; }


        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[a-zA-Z]+[ a-zA-Z-_]*$", ErrorMessage = "Use Characters only")]
        [StringLength(16, ErrorMessage = "Must be between 2 and 16 characters", MinimumLength = 2)]
        public string City { get; set; }


    }
}
