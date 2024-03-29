using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.AdminTab;
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

        DashTable GetPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype);
        DashTable ExportPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype);


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
        PhysicianCustom getPhyProfile(int phid);
        Admin getAdminProfile(string email);
        AdminCustom getAdminCustomProfile(string email);

        List<Adminregion> getAdminReg(int adId);
        void updatePass(string email, string pass);

        void updateAdminInfo(AdminCustom adminData, string email);
        void updateAdminLocation(Admin adminData, string email);
        bool CheckAdminReg(int adminId, int Regid);
        void addAdminRegion(int adminId, int regioId);
        void removeAdminReg(int adminId, int regionId);

        // admin create
        Aspnetuser CreteAdminAspnetUser(adminViewModel model);
        Admin CreateNewAdmin(adminViewModel model, int cretedBy, int aspId);


        //Physical Data
        List<Physician> getAllPhysicianData();

        string getPhysicianEmail(int phid);

        Physician getPhysicianDetail(int phid);


        // role
        List<Aspnetrole> getAllAspnetrole();

        // log
        IEnumerable<Emaillog> getEmailLogData();
        IEnumerable<Blockrequest> getallBlockRequest();


        // role
        List<Role> getAllroleDetails();
        Role AddRole(string roleName, short AccType, int aspId);
        void UpdateRole(int roleid, string roleName, short AccType, int aspId);
        bool checkRoleMenu(int roleId, int menuId);
        void addRoleMenu(int roleId, int menuId);
        void removeRoleMenu(int roleId, int menuId);


        // user access
        List<Aspnetuser> UserAccessData();


        // User table list
        List<User> getAllUserData();


        // Request whole data
        List<Request> getAllReqData();

    }
}
