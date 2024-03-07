using HalloDoc.Entity.AdminDash;
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

        //login
        int getAdminId(string email);
        string getAdminUserName(string email);



        Requestclient GetClientById(int id);



        //Dashbord table Data 
        IEnumerable<tableData> GetTableData(int page, int pageSize);

        IEnumerable<tableData> GetTableDataPending(int page, int pageSize);

        IEnumerable<tableData> GetTableDataActive(int page, int pageSize);

        IEnumerable<tableData> GetTableDataConclude(int page, int pageSize);

        IEnumerable<tableData> GetTableDataToclose(int page, int pageSize);

        IEnumerable<tableData> GetTableDataUnpaid(int page, int pageSize);


        List<int> TotalCountPatient();



        //note

        void addNote(int reqid, string note);

        ViewNotesViewModel getAllNotes(int reqid);

        List<Casetag> getAllCaseTag();

        void AddreqLogStatus(int reqid, string note, int adminId, short status);
        void AddreqLogStatus(int reqid, string note, short status, int adminId, int tranPhyId);

        void updateReqStatus(int reqid, short status);

        void updateReqStatusWithPhysician(int reqid, int phyId,short status);


        //region

        List<Region> getAllRegion();

        List<Physician> GetAvaliablePhysician(int regionId);


        //block req
        void AddBlockRequest(int reqId, string note);

        //delete doc
        void DeleteDocFile(string file, int reqid);


        // health profession
        List<Healthprofessionaltype> getAllHealthProfession();
        List<Healthprofessional> getHealthProfessionBussiness(int professionTypeId);
        Healthprofessional getVendorDetail(int vendorid);
        void AddOrderDetail(int vendorid, int adminid, int reqid, string prescription, int refil);
    }
}
