using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Entity.AdminTab
{
    public class CreateRoleViewModel
    {
        public List<Aspnetrole> aspnetrole { get; set; }

        public List<Menu> Menus { get; set; }   
    }
}
