using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public interface IGenral
    {
        bool CheckAvalibleRegion(string state);
        bool CheckExistAspUser(string email);
        bool CheckAspPassword(string email, string pass);
        Aspnetuser getUserRole(string email);

        int getAspId(string email);
        int getPhyId(string email);
        bool checkBlockReq(string email);

        string userFullName(string email);

        Requestclient GetClientById(int id);

        string getClientEmailbyReqId(int reqid);
        Request getRequestData(int reqid);

        void addEmailLog(string eTemplate, string sub, string recemail, string? filepath, int? roleid, int? reqid, int? adminid, int? phyid);
        void addSMSLog(string body, string mobile, int? roleid, int? reqid, int? adminid, int? phyid);

        EncounterForm getEncounterDetail(int reqid);
        bool CheckEncounterForm(int reqid);
        bool CheckEncounterFinalize(int reqid);
        void EncounterFinalize(int reqid);
        void AddEncounterForm(EncounterForm modeldata, int? AdminId, int? PhyId);
        void UpdateEncounterForm(EncounterForm modeldata, int? AdminId, int? PhyId);


        Task SendEmailOffice365(string recEmail, string subject, string body, List<string>? attachment);
        Task sendSMSwithTwilio(string mobile, string msg);

        //IEnumerable<RequestWithFile> GetRequestsFileswithReq(int reqId);
        List<Request> GetRequestsFileswithReq(int reqId);




        ClientInformation getClientProfile(string email);
        ClientInformation getUserProfile(string email);
        //update profile patient

        void UpdateUser(ClientInformation userInfo, string email);
        void UpdateRequestClient(ClientInformation userInfo, string email);
        void UpdateRequestClient(string clientEmail, string email, string phone);


        void AddDocFile(IFormFile DocFile, int reqId);


        void AddreqLogStatus(int reqid, string note, int? adminId, int? phyid, short status);
        void AddreqLogStatus(int reqid, string note, short status, int? adminId, int? phyid, int TransPhyid);

        void updateReqStatus(int reqid, short status);
        void updateReqStatusWithPhysician(int reqid, int? phyId, short status);

        List<Menu> getMenuNames(int? accType);
        Menu getSingleMenu(int accType, string name);
        Role getRoleinfo(int roleId);
        List<Rolemenu> getAllroleMenu(int roleId);
        void deleteRole(int roleid);
    }
}
