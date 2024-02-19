using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.RequestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{

    public interface IAdmin
    {
        IEnumerable<tableData> GetTableData();
    }
}
