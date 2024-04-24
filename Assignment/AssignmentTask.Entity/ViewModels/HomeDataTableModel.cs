using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentTask.Entity.ViewModels
{
    public class HomeDataTableModel
    {
        public IEnumerable<BookPopupViewModel> BookModelList { get; set; }

        public int? totalRecord { get; set; }
    }
}
