using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDash
{
    public class ViewNotesViewModel
    {
        public int RequestId { get; set; } 
        public List<Requeststatuslog> reqLog { get; set; }
        public List<Requestnote> reqNote { get; set; }
    }
}
