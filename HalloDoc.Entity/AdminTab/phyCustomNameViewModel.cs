using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class phyCustomNameViewModel
    {
        public int Physicianid { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public List<int> phyReg { get; set; }
    }
}
