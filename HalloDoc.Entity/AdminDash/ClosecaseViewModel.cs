using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDash
{
    public class ClosecaseViewModel
    {
        public ClientInformation ClientInformation { get; set; }

        public List<Request> requests { get; set; }
    }
}
