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

        public int getRegionId(string state)
        {
            return _context.Regions.FirstOrDefault(x => x.Name.ToLower() == state.ToLower()).Regionid;
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
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();
            return asp;
        }

        public void addAspnetUserrole(int aspId, int roleId)
        {
            Aspnetuserrole aspRole = new Aspnetuserrole
            {
                Userid = aspId,
                Roleid = roleId,
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
                Regionid = getRegionId(client.State),
                Street = client.Street,
                City = client.City,
                State = client.State,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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


        public string getRegAbr(string region)
        {
            return _context.Regions.FirstOrDefault(x => x.Name.ToLower() == region.ToLower()).Abbreviation.ToUpper();
        }

        public int DailyRequestCount()
        {
            return _context.Requests.Where(x => x.Createddate.Date == System.DateTime.Now.Date).ToList().Count();
        }

        public string getConfirmNum(string reg, string fname, string lname)
        {
            string reqCount = DailyRequestCount().ToString().PadLeft(4, '0');
            string num = getRegAbr(reg).Substring(0, 2) + System.DateTime.Now.ToString("dd") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yy") + fname.ToUpper().Substring(0, 2) + lname.ToUpper().Substring(0, 2) +  reqCount;
            return num;
        }


        public Request AddRequest(ClientInformation client, int userId)
        {
            Request req = new Request
            {
                Userid = userId,
                Requesttypeid = 2,
                Status = 1,
                Confirmationnumber = getConfirmNum(client.State, client.Firstname, client.Lastname),
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Phonenumber = client.Phonenumber,
                Email = client.Email,
                Symptoms = client.Symptoms,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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
                Regionid = getRegionId(fInfo.clientInformation.State),
                Street = fInfo.clientInformation.Street,
                City = fInfo.clientInformation.City,
                State = fInfo.clientInformation.State,
                Zipcode = fInfo.clientInformation.Zipcode,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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
                Confirmationnumber = getConfirmNum(fInfo.clientInformation.State, fInfo.clientInformation.Firstname, fInfo.clientInformation.Lastname),
                Locroom = fInfo.clientInformation.Locroom,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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
                Zipcode = fInfo.clientInformation.Zipcode,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
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
                Conciergeid = ConciergeId,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString()
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
                Businessid = BussinessId,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Requestbusinesses.Add(reqbus);
            _context.SaveChanges();
        }



        //create Patient
        public Aspnetuser createonlyAsp(ClientInformation user)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Username = user.Firstname + user.Lastname,
                Email = user.Email,
                Phonenumber = user.Phonenumber,
                Passwordhash = user.ConfirmPassword.GetHashCode().ToString(),
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();

            return asp;
        }

        public void updateUserIdWithAsp(int aspId, string email)
        {
            User user = _context.Users.FirstOrDefault(x => x.Email == email);
            user.Aspnetuserid = aspId;
            user.Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            _context.Users.Update(user);
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
            asp.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.ConfirmPassword);
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
