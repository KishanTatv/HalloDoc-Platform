using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Implement
{
    public class PatientRepo : IPatient
    {

        private readonly HalloDocDbContext _context;

        public PatientRepo(HalloDocDbContext context)
        {
            _context = context;

        }


        public bool CheckExistAspUser(string email)
        {
            return _context.Aspnetusers.Any(u => u.Email == email);
        }

        public Requestclient GetClientById(int id)
        {
            var userData = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == id);
            return userData;
        }

        public Aspnetuser AddAspUser(ClientInformation client)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Email = client.Email,
                Username = client.Firstname + client.Lastname,
                Passwordhash = client.Password,
                Phonenumber = client.Phonenumber,
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();
            return asp;
        }

        public User AddUser(ClientInformation client, int Aspid)
        {
            User user = new User
            {
                Aspnetuserid = Aspid,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Dob = client.Dob,
                Mobile = client.Phonenumber,
                Email = client.Email,
                Street = client.Street,
                City = client.City,
                State = client.State,
                Zipcode = client.Zipcode
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public int FindUserId(string email)
        {
            var UserId = _context.Users.FirstOrDefault(x => x.Email == email).Userid;
            return UserId;
        }

        public Request AddRequest(ClientInformation client, int userId)
        {
            Request req = new Request
            {
                Userid = userId,
                Requesttypeid = 2,
                Status = 1,
                Confirmationnumber = "M" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Phonenumber = client.Phonenumber,
                Email = client.Email,
                Locroom = client.Locroom,
                Symptoms = client.Symptoms,
            };
            _context.Requests.Add(req);
            _context.SaveChanges();
            return req;
        }

        public void AddRequestClient(ClientInformation client, int reqId)
        {
            Requestclient reqClient = new Requestclient
            {
                Requestid = reqId,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Phonenumber = client.Phonenumber,
                Dob = client.Dob,
                Email = client.Email,
                Street = client.Street,
                City = client.City,
                State = client.State,
                Zipcode = client.Zipcode,
            };
            _context.Requestclients.Add(reqClient);
            _context.SaveChanges();
        }


        public void AddDocFile(IFormFile DocFile, int reqId)
        {
            Requestwisefile reqFile = new Requestwisefile
            {
                Requestid = reqId,
                Filename = DocFile.FileName,
                Createddate = System.DateTime.Now,
                Doctype = 1
            };
            _context.Requestwisefiles.Add(reqFile);
            _context.SaveChanges();
        }



        //fcb three form
        public User AddFcbUser(FormFCB fInfo)
        {
            User user = new User
            {
                Firstname = fInfo.clientInformation.Firstname,
                Lastname = fInfo.clientInformation.Lastname,
                Mobile = fInfo.clientInformation.Phonenumber,
                Dob = fInfo.clientInformation.Dob,
                Email = fInfo.clientInformation.Email,
                Street = fInfo.clientInformation.Street,
                City = fInfo.clientInformation.City,
                State = fInfo.clientInformation.State,
                Zipcode = fInfo.clientInformation.Zipcode,
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public Request AddFcbRequest(FormFCB fInfo, int UserId, int reqType)
        {
            Request request = new Request
            {
                Userid = UserId,
                Requesttypeid = reqType,
                Status = 1,
                Firstname = fInfo.PatientFname,
                Lastname = fInfo.PatientLname,
                Phonenumber = fInfo.PatientPhonenumber,
                Email = fInfo.PatientEmail,
                Confirmationnumber = "M" + Guid.NewGuid().ToString().Substring(0, 9).ToUpper(),
                Locroom = fInfo.clientInformation.Locroom,
                Symptoms = fInfo.clientInformation.Symptoms
            };
            _context.Requests.Add(request);
            _context.SaveChanges();

            return request;
        }

        public void AddFcbRequestClient(FormFCB fInfo, int reqId)
        {
            Requestclient reqClient = new Requestclient
            {
                Requestid = reqId,
                Firstname = fInfo.clientInformation.Firstname,
                Lastname = fInfo.clientInformation.Lastname,
                Phonenumber = fInfo.clientInformation.Phonenumber,
                Dob = fInfo.clientInformation.Dob,
                Email = fInfo.clientInformation.Email,
                Street = fInfo.clientInformation.Street,
                City = fInfo.clientInformation.City,
                State = fInfo.clientInformation.State,
                Zipcode = fInfo.clientInformation.Zipcode
            };
            _context.Requestclients.Add(reqClient);
            _context.SaveChanges();
        }

        public Concierge AddConcierge(FormFCB fInfo)
        {
            Concierge concierge = new Concierge
            {
                Conciergename = fInfo.PatientFname + " " + fInfo.PatientLname,
                Street = fInfo.PatientStreet,
                City = fInfo.PatientCity,
                State = fInfo.PatientState,
                Zipcode = fInfo.PatientZipcode
            };
            _context.Concierges.Add(concierge);
            _context.SaveChanges();
            return concierge;
        }

        public void AddRequestConcierge(FormFCB fInfo, int ReqId, int ConciergeId)
        {
            Requestconcierge conReq = new Requestconcierge
            {
                Requestid = ReqId,
                Conciergeid = ConciergeId
            };
            _context.Requestconcierges.Add(conReq);
            _context.SaveChanges();
        }


        public Business AddBussiness(FormFCB fInfo)
        {
            Business bus = new Business
            {
                Name = fInfo.PatientFname + " " + fInfo.PatientLname,
                Phonenumber = fInfo.PatientPhonenumber
            };
            _context.Businesses.Add(bus);
            _context.SaveChanges();
            return bus;
        }

        public void AddRequestBussiness(FormFCB fInfo, int ReqId, int BussinessId)
        {
            Requestbusiness reqbus = new Requestbusiness
            {
                Requestid = ReqId,
                Businessid = BussinessId
            };
            _context.Requestbusinesses.Add(reqbus);
            _context.SaveChanges();
        }



        //create Patient
        public Aspnetuser createonlyAsp(Aspnetuser user)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Email = user.Email,
                Passwordhash = user.Passwordhash,
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();

            return asp;
        }

        public void updateUserIdWithAsp(int aspId, string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == email);
            user.Aspnetuserid = aspId;
            _context.Users.Update(user);
            _context.SaveChanges();
        }



        //update profile

        public void UpdateUser(PatientDash userInfo, string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            user.Firstname = userInfo.User.Firstname;
            user.Lastname = userInfo.User.Lastname;
            user.Mobile = userInfo.User.Phonenumber;
            user.Dob = userInfo.User.Dob;
            user.Street = userInfo.User.Street;
            user.City = userInfo.User.City;
            user.State = userInfo.User.State;
            user.Zipcode = userInfo.User.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateRequestClient(PatientDash userInfo,string email)
        {
            Requestclient reqClient = _context.Requestclients.FirstOrDefault(u => u.Email == email);
            reqClient.Firstname = userInfo.User.Firstname;
            reqClient.Lastname = userInfo.User.Lastname;
            reqClient.Phonenumber = userInfo.User.Phonenumber;
            reqClient.Dob = userInfo.User.Dob;
            reqClient.Street = userInfo.User.Street;
            reqClient.City = userInfo.User.City;
            reqClient.State = userInfo.User.State;
            reqClient.Zipcode = userInfo.User.Zipcode;
            _context.Requestclients.Update(reqClient);
            _context.SaveChanges();
        }




        //User full name
        public string userFullName(string email)
        {
             string userName = _context.Users.FirstOrDefault(u => u.Email.Equals(email)).Firstname + " "+ _context.Users.FirstOrDefault(u => u.Email.Equals(email)).Lastname; ;
            return userName;
        }

        //sendMailResetPassword
        public void sendMail(string email, string Sub, string bodyMsg)
        {
            var fromAddress = new MailAddress("kishanbhadani15@gmail.com", "HalloDoc Platform");
            var toAddress = new MailAddress(email);
            var subject = Sub;
            var body = bodyMsg;
            var message = new MailMessage(fromAddress, toAddress) 
            { 
                Subject = subject, 
                Body = body, 
                IsBodyHtml = true 
            };
            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("kishanbhadani15@gmail.com", "lvjv noea sxng licz"),
                    EnableSsl = true
                };
                smtpClient.Send(message); Console.WriteLine(" sending email: ");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        //new-Password Create
        public void newPasswordCreate(ClientInformation user, string email)
        {
            Aspnetuser asp = _context.Aspnetusers.FirstOrDefault(u => u.Email == email);
            asp.Passwordhash = user.ConfirmPassword;
            _context.Aspnetusers.Update(asp);
            _context.SaveChanges(); 
        }


        //specific data return
        public Requestclient GetUserByEmail(string email)
        {
            var userData = _context.Requestclients.FirstOrDefault(x => x.Email == email);
            return userData;
        }

        public IEnumerable<RequestWithFile> GetRequestsFiles(string email)
        {
            var reqFile = from r in _context.Requests
                          join f in _context.Users on r.Userid equals f.Userid into rf
                          from f in rf.DefaultIfEmpty()
                          where f.Email == email
                          orderby r.Createddate descending
                          select new RequestWithFile
                          {
                              Requestid = r.Requestid,
                              Createddate = r.Createddate,
                              Firstname = r.Firstname + " " + r.Lastname,
                              Status = r.Status,
                              FileCount = _context.Requestwisefiles.Where(u => u.Requestid == r.Requestid).Count(),
                          };
            return reqFile;
        }


        public IEnumerable<RequestWithFile> GetRequestsFileswithReq(string email, int reqId)
        {
            var reqFile = from u in _context.Users
                          join r in _context.Requests on u.Userid equals r.Userid
                          join f in _context.Requestwisefiles on r.Requestid equals f.Requestid into rf
                          from f in rf.DefaultIfEmpty()
                          where (r.Email == email) && (f.Requestid == reqId)
                          orderby r.Createddate descending
                          select new RequestWithFile
                          {
                              Requestwisefileid = f.Requestwisefileid,
                              Requestid = r.Requestid,
                              Createddate = r.Createddate,
                              Firstname = r.Firstname + " " + r.Firstname,
                              UploadDate = f.Createddate,
                              Status = r.Status,
                              Filename = f.Filename,
                          };
            return reqFile;
        }

    }
}
