using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HalloDoc.Entity.AdminTab
{
    public class ReciptWeeklySheet
    {

        public string date { get; set; }

        public string item { get; set; }

        public decimal amount { get; set; }

        public IFormFile? bill { get; set; }
    }
}
