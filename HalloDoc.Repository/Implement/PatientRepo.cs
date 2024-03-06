using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HalloDoc.Repository.Implement
{
    public class PatientRepo : IPatient
    {

        private readonly HalloDocDbContext _context;

        public PatientRepo(HalloDocDbContext context)
        {
            _context = context;

        }

        public string CheckAspPassword(string email)
        {
            return _context.Aspnetusers.FirstOrDefault(u => u.Email == email).Passwordhash;
        }

        public User GetUserById(int id)
        {
            var userData = _context.Users.FirstOrDefault(x => x.Userid == id);
            return userData;
        }

        public Aspnetuser AddAspUser(ClientInformation client)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Email = client.Email,
                Username = client.Firstname + " " + client.Lastname,
                Passwordhash = client.Password,
                Phonenumber = client.Phonenumber,
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();
            return asp;
        }

        public void AddAspnetUserRole(int aspid)
        {
            Aspnetuserrole aspRole = new Aspnetuserrole
            {
                Userid = aspid,
                Roleid = 3
            };
            _context.Aspnetuserroles.Add(aspRole);
            _context.SaveChanges();
        }

        public User AddUser(ClientInformation client, int Aspid)
        {
            User user = new User
            {
                Aspnetuserid = Aspid,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Intdate = client.Dob.Day,
                Strmonth = client.Dob.ToString("MMMM"),
                Intyear = client.Dob.Year,
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
                Intdate = client.Dob.Day,
                Strmonth = client.Dob.ToString("MMMM"),
                Intyear = client.Dob.Year,
                Email = client.Email,
                Notes = client.Symptoms,
                Street = client.Street,
                City = client.City,
                State = client.State,
                Zipcode = client.Zipcode,
            };
            _context.Requestclients.Add(reqClient);
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
                Intdate = fInfo.clientInformation.Dob.Day,
                Strmonth = fInfo.clientInformation.Dob.ToString("MMMM"),
                Intyear = fInfo.clientInformation.Dob.Year,
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
                Intdate = fInfo.clientInformation.Dob.Day,
                Strmonth = fInfo.clientInformation.Dob.ToString("MMMM"),
                Intyear = fInfo.clientInformation.Dob.Year,
                Email = fInfo.clientInformation.Email,
                Notes = fInfo.clientInformation.Symptoms,
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
        public Aspnetuser createonlyAsp(ClientInformation user)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Email = user.Email,
                Passwordhash = user.ConfirmPassword.GetHashCode().ToString(),
            };
            Console.WriteLine(user.ConfirmPassword.GetHashCode().ToString());
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



        // profile
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
                    Street = r.Street,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.Zipcode,
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
        public User GetUserByEmail(string email)
        {
            var userData = _context.Users.FirstOrDefault(x => x.Email == email);
            return userData;
        }

        public IEnumerable<RequestWithFile> GetRequestsFiles(string email)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;

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
                              FileCount = _context.Requestwisefiles.Where(u => u.Requestid == r.Requestid && u.Isdeleted == bitArray).Count(),
                          };
            return reqFile;
        }

    }
}
