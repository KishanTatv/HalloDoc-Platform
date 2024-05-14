using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class SheetPayrateModel
    {
        public Providerpayrate Providerpayrate { get; set; }

        public List<Providerweeklysheet> providerweeklysheets { get; set; }
    }
}
