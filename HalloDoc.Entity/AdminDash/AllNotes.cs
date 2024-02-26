using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDash
{
    public class AllNotes
    {
        public int RequestStatusLogId { get; set; }

        public string RequestId { get; set; }

        public Int16 Status { get; set; } 

        public int PhysicianId { get; set; }

        public int AdminId { get; set; }

        public int TransToPhysicianId { get; set; }

        public string Notes { get; set; }

        public int RequestNotesId { get; set; }

        public string PhysicianNotes { get; set; }

        public string AdminNotes  { get; set; }
        
    }
}
