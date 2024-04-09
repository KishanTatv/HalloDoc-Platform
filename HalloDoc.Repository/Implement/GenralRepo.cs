using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
using System.Web.Helpers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
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

        public bool CheckAspPassword(string email, string pass)
        {
            var Asppass = _context.Aspnetusers.FirstOrDefault(u => u.Email == email).Passwordhash;
            if(Crypto.VerifyHashedPassword(Asppass, pass))
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public int getAspId(string email)
        {
            return _context.Aspnetusers.FirstOrDefault(x => x.Email == email).Id;
        }

        public int getPhyId(string email)
        {
            return _context.Physicians.FirstOrDefault(x => x.Email == email).Physicianid;
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

        public Request getRequestData(int reqid)
        {
            return _context.Requests.FirstOrDefault(x => x.Requestid == reqid);
        }


        public void addEmailLog(string eTemplate, string sub, string recemail, string? filepath, int? roleid, int? reqid, int? adminid, int? phyid)
        {
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
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(new bool[] { true }),
                Senttries = 1,
            };
            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();
        }


        public void addSMSLog(string body, string mobile, int? roleid, int? reqid, int? adminid, int? phyid)
        {
            Smslog smslog = new Smslog
            {
                Smstemplate = body,
                Mobilenumber = mobile,
                Roleid = roleid,
                Requestid = reqid,
                Physicianid = phyid,
                Adminid = adminid,
                Senttries = 1,
                Issmssent = new BitArray(new bool[] { true }),
                Createdate = System.DateTime.Now,
                Sentdate = System.DateTime.Now,
            };
            _context.Smslogs.Add(smslog);
            _context.SaveChanges();
        }


        #region Encounter
        public EncounterForm getEncounterDetail(int reqid)
        {
            var data = _context.EncounterForms.FirstOrDefault(x => x.RequestId == reqid);
            return data;
        }

        public bool CheckEncounterForm(int reqid)
        {
            return _context.EncounterForms.Any(x => x.RequestId == reqid);
        }

        public bool CheckEncounterFinalize(int reqid)
        {
            if (CheckEncounterForm(reqid))
            {
                var checkFin = _context.EncounterForms.FirstOrDefault(x => x.RequestId == reqid).IsFinalize;
                if (checkFin == true)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public void EncounterFinalize(int reqid)
        {
            EncounterForm form = _context.EncounterForms.FirstOrDefault(x => x.RequestId == reqid);
            form.IsFinalize = true;
            _context.EncounterForms.Update(form);
            _context.SaveChanges();
        }

        public void AddEncounterForm(EncounterForm modeldata, int? AdminId, int? PhyId)
        {
            EncounterForm form = new EncounterForm
            {
                RequestId = modeldata.RequestId,
                HistoryOfPresentIllnessOrInjury = modeldata.HistoryOfPresentIllnessOrInjury,
                MedicalHistory = modeldata.MedicalHistory,
                Medications = modeldata.Medications,
                Allergies = modeldata.Allergies,
                Temp = modeldata.Temp,
                Hr = modeldata.Hr,
                Rr = modeldata.Rr,
                BloodPressureDiastolic = modeldata.BloodPressureDiastolic,
                BloodPressureSystolic = modeldata.BloodPressureSystolic,
                O2 = modeldata.O2,
                Pain = modeldata.Pain,
                Heent = modeldata.Heent,
                Cv = modeldata.Cv,
                Chest = modeldata.Chest,
                Abd = modeldata.Abd,
                Extremeties = modeldata.Extremeties,
                Skin = modeldata.Skin,
                Neuro = modeldata.Neuro,
                Other = modeldata.Other,
                Diagnosis = modeldata.Diagnosis,
                TreatmentPlan = modeldata.TreatmentPlan,
                MedicationsDispensed = modeldata.MedicationsDispensed,
                Procedures = modeldata.Procedures,
                FollowUp = modeldata.FollowUp,
                AdminId = AdminId,
                PhysicianId = PhyId,
            };
            _context.EncounterForms.Add(form);
            _context.SaveChanges();
        }

        public void UpdateEncounterForm(EncounterForm modeldata, int? AdminId, int? PhyId)
        {
            EncounterForm form = _context.EncounterForms.FirstOrDefault(x => x.RequestId == modeldata.RequestId);
            form.HistoryOfPresentIllnessOrInjury = modeldata.HistoryOfPresentIllnessOrInjury;
            form.MedicalHistory = modeldata.MedicalHistory;
            form.Medications = modeldata.Medications;
            form.Allergies = modeldata.Allergies;
            form.Temp = modeldata.Temp;
            form.Hr = modeldata.Hr;
            form.Rr = modeldata.Rr;
            form.BloodPressureDiastolic = modeldata.BloodPressureDiastolic;
            form.BloodPressureSystolic = modeldata.BloodPressureSystolic;
            form.O2 = modeldata.O2;
            form.Pain = modeldata.Pain;
            form.Heent = modeldata.Heent;
            form.Cv = modeldata.Cv;
            form.Chest = modeldata.Chest;
            form.Abd = modeldata.Abd;
            form.Extremeties = modeldata.Extremeties;
            form.Skin = modeldata.Skin;
            form.Neuro = modeldata.Neuro;
            form.Other = modeldata.Other;
            form.Diagnosis = modeldata.Diagnosis;
            form.TreatmentPlan = modeldata.TreatmentPlan;
            form.MedicationsDispensed = modeldata.MedicationsDispensed;
            form.Procedures = modeldata.Procedures;
            form.FollowUp = modeldata.FollowUp;
            form.AdminId = AdminId;
            form.PhysicianId = PhyId;
            _context.EncounterForms.Update(form);
            _context.SaveChanges();
        }

        #endregion



        #region SendMail Office365
        public Task SendEmailOffice365(string recEmail, string subject, string body, List<string>? attachment)
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

        #region SMS Twilio
        public Task sendSMSwithTwilio(string mobile, string msg)
        {
            try
            {
                string accountSid = "AC6c9974bdd968d358144abc9d665e7ddb";
                string authToken = "5e8d84b81e2f40ddc9ccb3caf5ab1f33";

                TwilioClient.Init(accountSid, authToken);

                var twilioMessage = MessageResource.Create(
                    body: msg,
                    from: new Twilio.Types.PhoneNumber("+19175127590"),
                    to: new Twilio.Types.PhoneNumber("+918866773923")
                );
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twilio Error");
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
        public ClientInformation getUserProfile(string email)
        {
            var data = _context.Users.Where(r => r.Email == email)
                .Select(r => new ClientInformation
                {
                    Firstname = r.Firstname,
                    Lastname = r.Lastname,
                    Email = r.Email,
                    Phonenumber = r.Mobile,
                    Dob = new DateTime((int)r.Intyear, DateTime.ParseExact(r.Strmonth, "MMMM", CultureInfo.CurrentCulture).Month, (int)r.Intdate),
                    date = System.DateTime.Now,
                    Street = r.Street,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.Zipcode,
                    Address = $"{r.Street}, {r.City}, {r.State}, {r.Zipcode}",
                }).FirstOrDefault();
            return data;
        }

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
                    Street = r.Street,
                    City = r.City,
                    State = r.State,
                    Zipcode = r.Zipcode,
                    Notes = r.Notes,
                    Address = $"{r.Street}, {r.City}, {r.State}, {r.Zipcode}",
                }).FirstOrDefault();
            return data;
        }

        public void UpdateUser(ClientInformation client, string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);
            user.Firstname = client.Firstname;
            user.Lastname = client.Lastname;
            user.Mobile = client.Phonenumber;
            user.Intdate = client.Dob.Day;
            user.Strmonth = client.Dob.ToString("MMMM");
            user.Intyear = client.Dob.Year;
            user.Regionid = getRegionId(client.State);
            user.Street = client.Street;
            user.City = client.City;
            user.State = client.State;
            user.Zipcode = client.Zipcode;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UpdateRequestClient(ClientInformation client, string email)
        {
            Requestclient reqClient = _context.Requestclients.FirstOrDefault(u => u.Email == email);
            reqClient.Firstname = client.Firstname;
            reqClient.Lastname = client.Lastname;
            reqClient.Phonenumber = client.Phonenumber;
            reqClient.Intdate = client.Dob.Day;
            reqClient.Strmonth = client.Dob.ToString("MMMM");
            reqClient.Intyear = client.Dob.Year;
            reqClient.Street = client.Street;
            reqClient.City = client.City;
            reqClient.State = client.State;
            reqClient.Zipcode = client.Zipcode;
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



        #region menu

        public List<Menu> getMenuNames(int? accType)
        {
            if (accType == 0)
            {
                return _context.Menus.ToList();
            }
            else
            {
                return _context.Menus.Where(x => x.Accounttype == accType).ToList();
            }
        }

        public Menu getSingleMenu(int accType, string name)
        {
            return _context.Menus.FirstOrDefault(x => x.Accounttype == accType || x.Name.Contains(name));
        }

        public Role getRoleinfo(int roleId)
        {
            return _context.Roles.FirstOrDefault(x => x.Roleid == roleId);
        }

        public List<Rolemenu> getAllroleMenu(int roleId)
        {
            return _context.Rolemenus.Where(x => x.Roleid == roleId).ToList();
        }

        public void deleteRole(int roleid)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            Role role = _context.Roles.FirstOrDefault(x => x.Roleid == roleid);
            role.Isdeleted = bitArray;
            _context.Roles.Update(role);
            _context.SaveChanges();
        }

        #endregion
    }
}
