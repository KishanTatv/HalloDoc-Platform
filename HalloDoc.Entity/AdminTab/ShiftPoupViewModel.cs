using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class ShiftPoupViewModel
    {
        public List<Region> Regions { get; set; }

        public int regId { get; set; }

        [Required(ErrorMessage = "Please select Physician")]
        public int phyid { get; set; }

        [Required(ErrorMessage = "Please select ShiftDate")]
        public DateOnly shiftdate { get; set; }
        public TimeOnly timeStart { get; set; }
        public TimeOnly timeEnd { get; set; }

        public bool isRepeat { get; set; }

        public int repeatTime { get; set; }


    }
}
