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
        Aspnetuser getUserRole(string email);

        bool checkBlockReq(string email);

        string userFullName(string email);

        Requestclient GetClientById(int id);

        string getClientEmailbyReqId(int reqid);

        void addEmailLog(string eTemplate, string sub, string recemail, string filepath, int roleid, int reqid, int? adminid, int? phyid);

        Task SendEmailOffice365(string recEmail, string subject, string body, List<string> attachment);

        //IEnumerable<RequestWithFile> GetRequestsFileswithReq(int reqId);
        List<Request> GetRequestsFileswithReq(int reqId);

        void AddDocFile(IFormFile DocFile, int reqId);
    }
}
