using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HalloDoc.Repository.Interface
{

    public interface IAdmin
    {

        //login
        int getAdminId(string email);
        string getAdminUserName(string email);



        Requestclient GetClientById(int id);



        //Dashbord table Data 
        IEnumerable<tableData> GetTableData();

        IEnumerable<tableData> GetTableDataPending();

        IEnumerable<tableData> GetTableDataActive();

        IEnumerable<tableData> GetTableDataConclude();

        IEnumerable<tableData> GetTableDataToclose();

        IEnumerable<tableData> GetTableDataUnpaid();



        List<int> TotalCountPatient();



        //note

        void addNote(int reqid, string note);

        ViewNotesViewModel getAllNotes(int reqid);

        List<Casetag> getAllCaseTag();





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


        // Admin Profile
        Admin getAdminProfile(string email);

        List<Adminregion> getAdminReg(int adId);
        void updatePass(string email, string pass);

        void updateAdminInfo(Admin adminData, string email);
        void updateAdminLocation(Admin adminData, string email);


        //Physical Data
        List<Physician> getAllPhysicianData();

        string getPhysicianEmail(int phid);

        Physician getPhysicianDetail(int phid);
    }
}
