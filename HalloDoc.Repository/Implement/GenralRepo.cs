using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HalloDoc.Repository.Implement
{
    public class GenralRepo : IGenral
    {
        private readonly HalloDocDbContext _context;
        private readonly IConfiguration _config;

        public GenralRepo(HalloDocDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public bool checkBlockReq(string email)
        {
            return _context.Blockrequests.Any(u => u.Email == email);
        }

        public Requestclient GetClientById(int id)
        {
            var userData = _context.Requestclients.FirstOrDefault(x => x.Requestid == id);
            return userData;
        }

        public string getClientEmailbyReqId(int reqid)
        {
            return _context.Requestclients.FirstOrDefault(x => x.Requestid == reqid).Email;
        }


        #region SendMail Office365
        public Task SendEmailOffice365(string recEmail, string subject, string body)
        {
            string SenderEmail = _config["credntial:Email"];
            string Pass = _config["credntial:Pass"];

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(SenderEmail, Pass),
            };

            try
            {
                return smtpClient.SendMailAsync(new MailMessage(from: SenderEmail, to: recEmail, subject, body));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }

        }
        #endregion

        #region not work left join GetRequestsFileswithReq
        //public IEnumerable<RequestWithFile> GetRequestsFileswithReq(int reqId)
        //{
        //    var reqFile = from r in _context.Requests
        //                  join f in _context.Requestwisefiles on r.Requestid equals f.Requestid into rf
        //                  from f in rf.DefaultIfEmpty()
        //                  where f.Requestid == reqId
        //                  orderby r.Createddate descending
        //                  select new RequestWithFile
        //                  {
        //                      Requestwisefileid = f.Requestwisefileid,
        //                      Requestid = r.Requestid,
        //                      Createddate = r.Createddate,
        //                      Firstname = r.Firstname + " " + r.Firstname,
        //                      UploadDate = f.Createddate,
        //                      Status = r.Status,
        //                      Filename = f.Filename,
        //                  };
        //    return reqFile;
        //}
        #endregion

        public List<Request> GetRequestsFileswithReq(int reqId)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;

            var allReqFile1 = _context.Requests.Where(x => x.Requestid == reqId).Include(x => x.Requestclients).Include(x => x.Requestwisefiles.Where(x => x.Isdeleted == bitArray)).ToList();

            return allReqFile1;
        }

        public void AddDocFile(IFormFile DocFile, int reqId)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;

            Requestwisefile reqFile = new Requestwisefile
            {
                Requestid = reqId,
                Filename = DocFile.FileName,
                Createddate = System.DateTime.Now,
                Isdeleted = bitArray,
                Doctype = 1
            };
            _context.Requestwisefiles.Add(reqFile);
            _context.SaveChanges();
        }

    }
}
