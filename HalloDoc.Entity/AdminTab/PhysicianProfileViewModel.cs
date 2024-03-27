using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalloDoc.Entity.RequestForm;

namespace HalloDoc.Entity.AdminTab
{
    public class PhysicianProfileViewModel
    {
        public Physician physician { get; set; }


        public  PhysicianCustom PhysicianCustom { get; set; }

        public ClientInformation clientInformation { get; set; }

        public List<Region> Regions { get; set; }

        public List<Role> Roles { get; set; }

        public int phid { get; set; }
    }
}
