using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class UserAccessViewModel
    {
        public int aspId { get; set; }

        public int roleId { get; set; }

        public string userName { get; set; }

        public string Phone { get; set; }

        public int status { get; set; }

        public int totalRequest { get; set; }
    }
}
