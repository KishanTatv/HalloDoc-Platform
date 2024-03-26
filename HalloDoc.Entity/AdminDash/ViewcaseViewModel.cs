using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminDash
{
    public class ViewcaseViewModel
    {
        public Request request { get; set; }

        public ClientInformation clientInformation { get; set; }
    }
}
