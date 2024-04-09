using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class ProviderCallViewModel
    {
        public IEnumerable<ProcallModel> ProOncall { get; set; } = new List<ProcallModel>();

        public IEnumerable<ProcallModel> ProOffcall { get; set; } = new List<ProcallModel>();
    }
}
