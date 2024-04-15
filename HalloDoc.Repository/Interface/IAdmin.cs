using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        DashTable GetPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype, int? phyFilterId);
        DashTable ExportPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int reqtype);



        List<int> TotalCountPatient(int phyFilterId);
        void isdeleteReq(int reqId, BitArray isdelete);


        //note

        void addNote(int reqid, string? adminnote, string? phynote);

        ViewNotesViewModel getAllNotes(int reqid);

        List<Casetag> getAllCaseTag();





        //region

        List<Region> getAllRegion();

        List<Physicianregion> GetAvaliablePhysician(int regionId);
        List<Physicianregion> getRegionFromPhyID(int phyId);



        //block req
        void AddBlockRequest(int reqId, string note);
        void removeBlockRequest(int reqId);

        //delete doc
        void DeleteDocFile(string file, int reqid);


        // health profession
        void addHealthProfesion(VenderBusinessViewModel model);
        void updateHeaithProfession(VenderBusinessViewModel model);
        void deleteVendor(int venId);
        List<Healthprofessionaltype> getAllHealthProfession();
        List<Healthprofessional> getHealthProfessionBussiness(int professionTypeId);
        Healthprofessional getVendorDetail(int vendorid);
        void AddOrderDetail(int vendorid, int aspId, int reqid, string prescription, int refil);



        // provider Location
        IEnumerable<Physicianlocation> getAllPhysicianLocation();



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
        string getPhysicianMobile(int phid);

        Physician getPhysicianDetail(int phid);


        // role
        List<Aspnetrole> getAllAspnetrole();

        // log
        IEnumerable<Emaillog> getEmailLogData();
        IEnumerable<Smslog> getSMSLogData();
        IEnumerable<Blockrequest> getallBlockRequest();


        // role
        List<Role> getAllroleDetails();
        Role AddRole(string roleName, short AccType, int aspId);
        void UpdateRole(int roleid, string roleName, short AccType, int aspId);
        bool checkRoleMenu(int roleId, int menuId);
        void addRoleMenu(int roleId, int menuId);
        void removeRoleMenu(int roleId, int menuId);


        // user access
        IEnumerable<UserAccessViewModel> userAccessData();


        // User table list
        List<User> getAllUserData(string fname, string lname, string email, string phone);


        // Request whole data
        List<Request> getAllReqData(string? reqStatus, int reqType);


        // shift schedule
        Shift addNewShift(ShiftPoupViewModel model, string weekdays, int aspId);

        bool checkExistShift(ShiftPoupViewModel model, DateOnly newDate);
        Shiftdetail addNewShiftDetail(int shiftId, DateOnly shiftDate, ShiftPoupViewModel model, short status);
        void addShiftdetailRegion(int shiftDetailId, int regionId);

        List<phyCustomNameViewModel> getAllPhysicianName();

        IEnumerable<Shiftdetail> getAllShiftdetail(int reg);

        ShiftPoupViewModel getShiftDetailSpecific(int evenetId);

        void deleteShiftDetail(int shiftdetailId);

        void changeShiftdetailStatus(int shiftdetailId, short status);

        void updateShiftDetail(ShiftPoupViewModel model, int aspId);


        // MD on/off call
        IEnumerable<ProcallModel> phyOncallAvialble(int reg);
        IEnumerable<ProcallModel> phyOffcall(int reg);


    }
}
