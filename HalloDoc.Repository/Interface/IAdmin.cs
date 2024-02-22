using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
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

        Requestclient GetClientById(int id);

        IEnumerable<tableData> GetTableData(int StatusId, int page, int pageSize);

        IEnumerable<tableData> GetTableDataPending(int StatusId);

        IEnumerable<tableData> GetTableDataActive();

        IEnumerable<tableData> GetTableDataConclude();

        IEnumerable<tableData> GetTableDataToclose();

        IEnumerable<tableData> GetTableDataUnpaid();


        List<int> TotalCountPatient();
    }
}
