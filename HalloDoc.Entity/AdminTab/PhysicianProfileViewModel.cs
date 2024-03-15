using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class PhysicianProfileViewModel
    {
        public Physician physician { get; set; }
        public List<Region> Regions { get; set; }

        public int phid { get; set; }
    }
}
