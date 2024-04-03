using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class ProOncallModel
    {
        public int phyId { get; set; }

        public string physicianFName { get; set; }
        public string physicianLName { get; set; }

        public string photo { get; set; }

        public TimeOnly startTime { get; set; }

        public TimeOnly endTime { get; set; }
    }
}
