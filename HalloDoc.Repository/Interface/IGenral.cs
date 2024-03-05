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
        bool checkBlockReq(string email);

        string userFullName(string email);

        Requestclient GetClientById(int id);

        string getClientEmailbyReqId(int reqid);

        Task SendEmailOffice365(string recEmail, string subject, string body, List<string> attachment);

        //IEnumerable<RequestWithFile> GetRequestsFileswithReq(int reqId);
        List<Request> GetRequestsFileswithReq(int reqId);

        void AddDocFile(IFormFile DocFile, int reqId);

        

    }
}
