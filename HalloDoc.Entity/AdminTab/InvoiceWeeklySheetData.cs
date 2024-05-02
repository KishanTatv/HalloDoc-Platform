using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class InvoiceWeeklySheetData
    {
        public string date { get; set; }

        public bool isHoliday { get; set; }
        public int Totalhours { get; set; }
        public int housecall { get; set; }
        public int consult { get; set; }
    }
}
