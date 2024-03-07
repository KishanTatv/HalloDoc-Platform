using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDash
{
    public class OrderViewModel
    {
        public int requestId { get ; set; }

        public IEnumerable<Healthprofessionaltype> Healthprofessionaltype { get; set; }

        public IEnumerable<Healthprofessional> Healthprofessional { get; set; }

        public Healthprofessional vendorDetail { get; set; }

    }
}
