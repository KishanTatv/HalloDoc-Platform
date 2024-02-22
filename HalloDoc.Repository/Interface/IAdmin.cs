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

        IEnumerable<tableData> GetTableData(int StatusId);

        IEnumerable<tableData> GetTableWithPhyData(int StatusId);

        int TotalCountPatient(int statusId);
    }
}
