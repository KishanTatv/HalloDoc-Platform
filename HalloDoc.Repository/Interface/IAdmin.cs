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

        Request GetClientById(int id);



        //Dashbord table Data 
        IEnumerable<tableData> GetTableData(int page, int pageSize);

        IEnumerable<tableData> GetTableDataPending(int page, int pageSize);

        IEnumerable<tableData> GetTableDataActive(int page, int pageSize);

        IEnumerable<tableData> GetTableDataConclude(int page, int pageSize);

        IEnumerable<tableData> GetTableDataToclose(int page, int pageSize);

        IEnumerable<tableData> GetTableDataUnpaid(int page, int pageSize);


        List<int> TotalCountPatient();
    }
}
