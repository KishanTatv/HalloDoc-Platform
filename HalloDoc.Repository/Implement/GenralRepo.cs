using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Numerics;
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

        public bool CheckAvalibleRegion(string state)
        {
            return _context.Regions.Any(x => x.Name.ToLower() == state.ToLower());
        }


        public bool CheckExistAspUser(string email)
        {
            return _context.Aspnetusers.Any(u => u.Email == email);
        }

        public int getRegionId(string state)
        {
            return _context.Regions.FirstOrDefault(x => x.Name.ToLower() == state.ToLower()).Regionid;
        }

        public Aspnetuser getUserRole(string email)
        {
            var user = _context.Aspnetusers.Include(x => x.Aspnetuserrole).ThenInclude(x => x.Role).FirstOrDefault(x => x.Email == email);
            return user;
        }

        public bool checkBlockReq(string email)
        {
            return _context.Blockrequests.Any(u => u.Email == email);
        }

        //User full name
        public string userFullName(string email)
        {
            string userName = _context.Aspnetusers.FirstOrDefault(u => u.Email.Equals(email)).Username;
            return userName;
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


        public void addEmailLog(string eTemplate, string sub, string recemail, string filepath, int roleid, int? reqid, int? adminid, int? phyid)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;

            Emaillog emailLog = new Emaillog
            {
                Emailtemplate = eTemplate,
                Subjectname = sub,
                Emailid = recemail,
                Roleid = roleid,
                Requestid = reqid,
                Adminid = Convert.ToInt32(adminid),
                Physicianid = phyid,
                Filepath = filepath,
                Sentdate = DateTime.Now,
                Isemailsent = bitArray,
                Senttries = 1,
            };
            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();
        }


        #region Encounter
        public EncounterForm getEncounterDetail(int reqid)
        {
            var data = _context.EncounterForms.FirstOrDefault(x => x.RequestId == reqid);
            return data;
        }

        
        #endregion



        #region SendMail Office365
        public Task SendEmailOffice365(string recEmail, string subject, string body, List<string> attachment)
        {
            string SenderEmail = _config["credntial:Email"];
            string Pass = _config["credntial:Pass"];

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SenderEmail, Pass),
            };

            try
            {
                var fromAddress = new MailAddress(SenderEmail, "HalloDoc Platform");
                var toAddress = new MailAddress(recEmail);
                var mailMsg = new MailMessage(fromAddress, toAddress);
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                if (attachment != null)
                {
                    foreach (var file in attachment)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file);
                        if (System.IO.File.Exists(filePath))
                        {
                            mailMsg.Attachments.Add(new Attachment(filePath));
                        }
                    }
                }
                return smtpClient.SendMailAsync(mailMsg);
            }
            catch (Exception ex)
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




        #region statuslog
        public void AddreqLogStatus(int reqid, string note, int? adminId, int? phyid, short status)
        {
            Requeststatuslog reqStatus = new Requeststatuslog()
            {
                Requestid = reqid,
                Notes = note,
                Status = status,
                Adminid = adminId,
                Physicianid = phyid,
                Createddate = System.DateTime.Now
            };
            _context.Requeststatuslogs.Add(reqStatus);
            _context.SaveChanges();
        }

        public void AddreqLogStatus(int reqid, string note, short status, int? adminId, int? phyid, int TransPhyid)
        {
            Requeststatuslog reqStatus = new Requeststatuslog()
            {
                Requestid = reqid,
                Notes = note,
                Status = status,
                Adminid = adminId,
                Physicianid = phyid,
                Transtophysicianid = phyid,
                Createddate = System.DateTime.Now
            };
            _context.Requeststatuslogs.Add(reqStatus);
            _context.SaveChanges();
        }

        public void updateReqStatus(int reqid, short status)
        {
            Request req = _context.Requests.FirstOrDefault(x => x.Requestid == reqid);
            req.Status = status;
            req.Modifieddate = System.DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        public void updateReqStatusWithPhysician(int reqid, int phyId, short status)
        {
            Request req = _context.Requests.FirstOrDefault(x => x.Requestid == reqid);
            req.Status = status;
            req.Physicianid = phyId;
            req.Modifieddate = System.DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }

        #endregion



        #region patient Profile
        public ClientInformation getClientProfile(string email)
        {
            var data = _context.Requestclients.Where(r => r.Email == email)
                .Select(r => new ClientInformation
                {
                    Firstname = r.Firstname,
                    Lastname = r.Lastname,
                    Email = r.Email,
                    Phonenumber = r.Phonenumber,
                    Dob = new DateTime((int)r.Intyear, DateTime.ParseExact(r.Strmonth, "MMMM", CultureInfo.CurrentCulture).Month, (int)r.Intdate),
                    date = System.DateTime.Now,
                    Notes = r.Notes,
                    Street = r.Street,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.Zipcode,
                    Address = $"{r.Street}, {r.City}, {r.State}, {r.Zipcode}",
                }).FirstOrDefault();
            return data;
        }

        public void UpdateUser(PatientDash userInfo, string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            user.Firstname = userInfo.clientInfo.Firstname;
            user.Lastname = userInfo.clientInfo.Lastname;
            user.Mobile = userInfo.clientInfo.Phonenumber;
            user.Intdate = userInfo.clientInfo.Dob.Day;
            user.Strmonth = userInfo.clientInfo.Dob.ToString("MMMM");
            user.Intyear = userInfo.clientInfo.Dob.Year;
            user.Regionid = getRegionId(userInfo.clientInfo.State);
            user.Street = userInfo.clientInfo.Street;
            user.City = userInfo.clientInfo.City;
            user.State = userInfo.clientInfo.State;
            user.Zipcode = userInfo.clientInfo.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateRequestClient(PatientDash userInfo, string email)
        {
            Requestclient reqClient = _context.Requestclients.FirstOrDefault(u => u.Email == email);
            reqClient.Firstname = userInfo.clientInfo.Firstname;
            reqClient.Lastname = userInfo.clientInfo.Lastname;
            reqClient.Phonenumber = userInfo.clientInfo.Phonenumber;
            reqClient.Intdate = userInfo.clientInfo.Dob.Day;
            reqClient.Strmonth = userInfo.clientInfo.Dob.ToString("MMMM");
            reqClient.Intyear = userInfo.clientInfo.Dob.Year;
            reqClient.Street = userInfo.clientInfo.Street;
            reqClient.City = userInfo.clientInfo.City;
            reqClient.State = userInfo.clientInfo.State;
            reqClient.Zipcode = userInfo.clientInfo.Zipcode;
            _context.Requestclients.Update(reqClient);
            _context.SaveChanges();
        }

        public void UpdateRequestClient(string clientEmail, string email, string phone)
        {
            Requestclient reqClient = _context.Requestclients.FirstOrDefault(u => u.Email == clientEmail);
            reqClient.Email = email;
            reqClient.Phonenumber = phone;
            _context.Requestclients.Update(reqClient);
            _context.SaveChanges();
        }
        #endregion

    }
}
