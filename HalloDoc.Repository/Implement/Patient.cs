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
    public class Patient : IPatient
    {

        private readonly HalloDocDbContext _context;

        public Patient(HalloDocDbContext context)
        {
            _context = context;

        }


        public bool CheckExistAspUser(string email)
        {
            return _context.Aspnetusers.Any(u => u.Email == email);
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
                Email = client.Email,
                Street = client.Street,
                City = client.City,
                State = client.State,
                Zipcode = client.Zipcode,
            };
            _context.Requestclients.Add(reqClient);
            _context.SaveChanges();
        }


        public Request AddOnlyRequest(ClientInformation client)
        {
            Request req = new Request
            {
                Userid = _context.Users.FirstOrDefaultAsync(x => x.Email == client.Email).Result.Userid,
                Confirmationnumber = "M" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Requesttypeid = 2,
                Status = 1,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Phonenumber = client.Phonenumber,
                Email = client.Email,
                Locroom = client.Locroom,
                Symptoms = client.Symptoms,
                Relationname = client.relation,
            };
            _context.Requests.Add(req);
            _context.SaveChanges();

            return req;
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
                Dob = fInfo.clientInformation.Dob,
                Mobile = fInfo.clientInformation.Phonenumber,
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


        public Request AddOnlyFcbRequest(FormFCB fInfo, int reqType)
        {
            Request req = new Request
            {
                Userid = _context.Users.FirstOrDefault(u => u.Email == fInfo.clientInformation.Email).Userid,
                Requesttypeid = reqType,
                Status = 1,
                Firstname = fInfo.PatientFname,
                Lastname = fInfo.PatientLname,
                Phonenumber = fInfo.PatientPhonenumber,
                Email = fInfo.PatientEmail,
                Confirmationnumber = "M" + Guid.NewGuid().ToString().Substring(0, 9).ToUpper(),
                Locroom = fInfo.clientInformation.Locroom,
                Symptoms = fInfo.clientInformation.Symptoms,
            };
            _context.Requests.Add(req);
            _context.SaveChanges();
            return req;
        }



        //create Patient
        public void createPatient(Aspnetuser user)
        {
            Aspnetuser newUser = new Aspnetuser
            {
                Email = user.Email,
                Passwordhash = user.Passwordhash,
            };
            _context.Aspnetusers.Add(newUser);
            _context.SaveChanges();
        }



        //update profile
        public Aspnetuser UpdateAspUser(PatientDash userInfo, string email)
        {
            Aspnetuser asp = _context.Aspnetusers.FirstOrDefault(u => u.Email == email);
            asp.Email = userInfo.User.Email;
            asp.Username = userInfo.User.Firstname + userInfo.User.Lastname;
            asp.Phonenumber = userInfo.User.Mobile;
            _context.SaveChanges();

            return asp;
        }

        public void UpdateUser(PatientDash userInfo, string email, int aspId)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            user.Aspnetuserid = aspId;
            user.Firstname = userInfo.User.Firstname;
            user.Lastname = userInfo.User.Lastname;
            user.Dob = userInfo.User.Dob;
            user.Mobile = userInfo.User.Mobile;
            user.Email = userInfo.User.Email;
            user.Street = userInfo.User.Street;
            user.City = userInfo.User.City;
            user.State = userInfo.User.State;
            user.Zipcode = userInfo.User.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();
        }




        //User full name
        public string userFullName(Aspnetuser user)
        {
            string userName = _context.Users.FirstOrDefault(u => u.Email.Equals(user.Email)).Firstname + _context.Users.FirstOrDefault(u => u.Email.Equals(user.Email)).Lastname; ;
            return userName;
        }

        //sendMailResetPassword
        public void sendMailResetPassword(Aspnetuser user, string Sub, string bodyMsg)
        {
            var fromAddress = new MailAddress("kishanbhadani15@gmail.com", "HalloDoc Platform");
            var toAddress = new MailAddress(user.Email);
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
        public User GetUserByEmail(string email)
        {
            var userData = _context.Users.FirstOrDefault(x => x.Email == email);
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
