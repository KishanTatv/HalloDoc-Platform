using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class ProviderCallViewModel
    {
        public IEnumerable<ProOncallModel> ProOncall { get; set; } = new List<ProOncallModel>();

        public IEnumerable<ProOffcallModel> ProOffcall { get; set; } = new List<ProOffcallModel>();
    }
}
