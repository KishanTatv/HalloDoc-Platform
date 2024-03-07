using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDashTable
{
    public class popupModel
    {
        public Requestclient Requestclient { get; set; }
        public List<Casetag> Casetags { get; set; }
        public List<Region> Regions { get; set; }
    }
}
