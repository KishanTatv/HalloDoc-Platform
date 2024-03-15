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
        Aspnetuser getUserRole(string email);

        int getAspId(string email);
        bool checkBlockReq(string email);

        string userFullName(string email);

        Requestclient GetClientById(int id);

        string getClientEmailbyReqId(int reqid);

        void addEmailLog(string eTemplate, string sub, string recemail, string filepath, int roleid, int? reqid, int? adminid, int? phyid);
        
        EncounterForm getEncounterDetail(int reqid);

        Task SendEmailOffice365(string recEmail, string subject, string body, List<string> attachment);

        //IEnumerable<RequestWithFile> GetRequestsFileswithReq(int reqId);
        List<Request> GetRequestsFileswithReq(int reqId);




        ClientInformation getClientProfile(string email);
        ClientInformation getUserProfile(string email);
        //update profile patient

        void UpdateUser(PatientDash userInfo, string email);
        void UpdateRequestClient(PatientDash userInfo, string email);
        void UpdateRequestClient(string clientEmail, string email, string phone);


        void AddDocFile(IFormFile DocFile, int reqId);


        void AddreqLogStatus(int reqid, string note, int? adminId, int? phyid, short status);
        void AddreqLogStatus(int reqid, string note, short status, int? adminId, int? phyid, int TransPhyid);

        void updateReqStatus(int reqid, short status);

        void updateReqStatusWithPhysician(int reqid, int phyId, short status);
    }
}
