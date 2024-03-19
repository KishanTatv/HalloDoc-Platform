using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class ProfileViewModel
    {
        public Admin Admin { get; set; }

        public List<Region> Regions { get; set; }

        public List<Adminregion> AdminRegions { get; set; } 

    }
}
