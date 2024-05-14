using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.WebPages;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalloDoc.Repository.Implement
{
    public class AdminRepo : IAdmin
    {
        private readonly HalloDocDbContext _context;

        public AdminRepo(HalloDocDbContext context)
        {
            _context = context;

        }


        public string getAdminUserName(string email)
        {
            string fName = _context.Admins.FirstOrDefault(u => u.Email == email).Firstname;
            string lName = _context.Admins.FirstOrDefault(u => u.Email == email).Lastname;
            string name = fName + " " + lName;
            return name;
        }

        public int getAdminId(string email)
        {
            return _context.Admins.FirstOrDefault(u => u.Email == email).Adminid;
        }

        public Requestclient GetClientById(int id)
        {
            var reqData = _context.Requestclients.Where(x => x.Requestid == id).Include(x => x.Request).FirstOrDefault();
            return reqData;
        }

        public void isdeleteReq(int reqId, BitArray isdelete)
        {
            Request req = _context.Requests.FirstOrDefault(x => x.Requestid == reqId);
            req.Isdeleted = isdelete;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }


        #region Request Count
        public List<int> TotalCountPatient(int phyFilterId)
        {
            BitArray truebit = new BitArray(1);
            truebit[0] = true;
            int Tnew = _context.Requests.Count(x => x.Status == 1 && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));
            int Tpending = _context.Requests.Count(x => x.Status == 2 && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));
            int Tactive = _context.Requests.Count(x => (x.Status == 4 || x.Status == 5) && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));
            int Tconclide = _context.Requests.Count(x => x.Status == 6 && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));
            int Tclose = _context.Requests.Count(x => (x.Status == 3 || x.Status == 7 || x.Status == 8) && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));
            int Tunpaid = _context.Requests.Count(x => x.Status == 9 && x.Isdeleted != truebit && (phyFilterId == 0 || x.Physicianid.Equals(phyFilterId)));

            List<int> countList = new List<int> { Tnew, Tpending, Tactive, Tconclide, Tclose, Tunpaid };

            return countList;
        }
        #endregion

        #region AllRequest Data

        public DashTable GetPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype, int? phyFilterId)
        {
            var data = from r in _context.Requests
                       join f in _context.Requestclients on r.Requestid equals f.Requestid
                       join p in _context.Physicians on r.Physicianid equals p.Physicianid into rf
                       from p in rf.DefaultIfEmpty()
                       orderby r.Createddate descending
                       where (status == null ? true : status.Contains(r.Status)) && (r.Isdeleted != new BitArray(new bool[] { true })) &&
                      (search == null ? true : f.Firstname.ToLower().Contains(search.ToLower()) || f.Lastname.ToLower().Contains(search.ToLower())) &&
                      (reg == null ? true : f.State.ToLower().Equals(reg.ToLower())) &&
                      (phyFilterId == 0 ? true : p.Physicianid.Equals(phyFilterId)) &&
                      (reqtype == 0 ? true : r.Requesttypeid.Equals(reqtype))
                       select new tableData
                       {
                           Name = f.Firstname + " " + f.Lastname,
                           Intdate = f.Intdate,
                           Strmonth = f.Strmonth,
                           Intyear = f.Intyear,
                           Age = System.DateTime.Now.Year - f.Intyear,
                           Email = f.Email,
                           RequestId = f.Requestid,
                           ReqClientId = f.Requestclientid,
                           ReqTypeId = r.Requesttypeid,
                           status = r.Status,
                           Requestor = r.Firstname + "," + r.Lastname,
                           RequestedDate = r.Createddate,
                           PhysicianId = r.Physicianid,
                           PhysicianName = p.Firstname + " " + p.Lastname,
                           DateOfService = r.Accepteddate,
                           Phonenumber = f.Phonenumber,
                           ReqPhonenumber = r.Phonenumber,
                           Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                           Region = f.State,
                           Notes = f.Notes,
                           ReqNotes = _context.Requeststatuslogs.Where(x => x.Requestid == r.Requestid).OrderByDescending(x => x.Requeststatuslogid).FirstOrDefault().Notes,
                       };
            var tableData = new DashTable { Tdata = data.Skip((int)(page * pageSize)).Take(5).ToList(), filterCount = data.Count() };
            return tableData;
        }

        public DashTable ExportPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int reqtype)
        {
            var data = from r in _context.Requests
                       join f in _context.Requestclients on r.Requestid equals f.Requestid
                       join p in _context.Physicians on r.Physicianid equals p.Physicianid into rf
                       from p in rf.DefaultIfEmpty()
                       orderby r.Createddate descending
                       where (status == null ? true : status.Contains(r.Status)) &&
                      (search == null ? true : f.Firstname.ToLower().Contains(search.ToLower()) || f.Lastname.ToLower().Contains(search.ToLower())) &&
                      (reg == null ? true : f.State.ToLower().Equals(reg.ToLower())) &&
                      (reqtype == 0 ? true : r.Requesttypeid.Equals(reqtype))
                       select new tableData
                       {
                           Name = f.Firstname + " " + f.Lastname,
                           Intdate = f.Intdate,
                           Strmonth = f.Strmonth,
                           Intyear = f.Intyear,
                           Age = System.DateTime.Now.Year - f.Intyear,
                           Email = f.Email,
                           RequestId = f.Requestid,
                           ReqClientId = f.Requestclientid,
                           ReqTypeId = r.Requesttypeid,
                           Requestor = r.Firstname + "," + r.Lastname,
                           RequestedDate = r.Createddate,
                           PhysicianName = p.Firstname + " " + p.Lastname,
                           DateOfService = r.Accepteddate,
                           Phonenumber = f.Phonenumber,
                           ReqPhonenumber = r.Phonenumber,
                           Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                           Region = f.State,
                           Notes = f.Notes,
                           ReqNotes = _context.Requeststatuslogs.Where(x => x.Requestid == r.Requestid).OrderByDescending(x => x.Requeststatuslogid).FirstOrDefault().Notes,
                       };

            var tableData = new DashTable { Tdata = data.ToList(), filterCount = data.Count() };
            return tableData;
        }
        #endregion


        public void addNote(int reqid, string? adminnote, string? phynote)
        {
            Requestnote reqNote = new Requestnote()
            {
                Requestid = reqid,
                Adminnotes = adminnote,
                Physiciannotes = phynote,
                Createddate = DateTime.Now,
                Createdby = "yash"
            };
            _context.Requestnotes.Add(reqNote);
            _context.SaveChanges();
        }

        public ViewNotesViewModel getAllNotes(int reqid)
        {
            List<Requeststatuslog> reqLog = _context.Requeststatuslogs.Where(x => x.Requestid == reqid).ToList();
            List<Requestnote> reqNote = _context.Requestnotes.Where(x => x.Requestid == reqid).ToList();
            ViewNotesViewModel viewNote = new ViewNotesViewModel();
            viewNote.RequestId = reqid;
            viewNote.reqLog = reqLog;
            viewNote.reqNote = reqNote;
            return viewNote;
        }

        public List<Casetag> getAllCaseTag()
        {
            var CaseTagData = _context.Casetags.ToList();
            return CaseTagData;
        }

        public List<Region> getAllRegion()
        {
            var region = _context.Regions.ToList();
            return region;
        }

        public List<Physicianregion> GetAvaliablePhysician(int regionId)
        {
            var phyList = _context.Physicianregions.Include(x => x.Physician).Where(x => x.Regionid == regionId).ToList();
            return phyList;
        }

        public List<Physicianregion> getRegionFromPhyID(int phyId)
        {
            return _context.Physicianregions.Include(x => x.Region).Where(x => x.Physicianid == phyId).ToList();
        }


        public void AddBlockRequest(int reqId, string note)
        {
            string email = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Email;
            string phoneNum = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Phonenumber;

            Blockrequest req = new Blockrequest()
            {
                Requestid = reqId,
                Reason = note,
                Email = email,
                Phonenumber = phoneNum,
                Isactive = new BitArray(new bool[] { true }),
                Createddate = System.DateTime.Now,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Blockrequests.Add(req);
            _context.SaveChanges();
        }

        public void DeleteDocFile(string file, int reqid)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;

            Requestwisefile filedata = _context.Requestwisefiles.FirstOrDefault(u => u.Requestid == reqid && u.Filename == file);
            filedata.Isdeleted = bitArray;
            _context.Requestwisefiles.Update(filedata);
            _context.SaveChanges();
        }



        // health Profession

        public void addHealthProfesion(VenderBusinessViewModel model)
        {
            Healthprofessional healthprofessional = new Healthprofessional
            {
                Vendorname = model.Vendorname,
                Profession = model.Profession,
                Phonenumber = model.Phonenumber,
                Email = model.Email,
                Businesscontact = model.Businesscontact,
                Faxnumber = model.Faxnumber,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Zip = model.Zip,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
                Createddate = System.DateTime.Now,
            };
            _context.Healthprofessionals.Add(healthprofessional);
            _context.SaveChanges();
        }

        public void updateHeaithProfession(VenderBusinessViewModel model)
        {
            Healthprofessional healthprofessional = _context.Healthprofessionals.FirstOrDefault(x => x.Vendorid == model.Vendorid);
            healthprofessional.Vendorname = model.Vendorname;
            healthprofessional.Profession = model.Profession;
            healthprofessional.Phonenumber = model.Phonenumber;
            healthprofessional.Email = model.Email;
            healthprofessional.Businesscontact = model.Businesscontact;
            healthprofessional.Faxnumber = model.Faxnumber;
            healthprofessional.Address = model.Address;
            healthprofessional.City = model.City;
            healthprofessional.State = model.State;
            healthprofessional.Zip = model.Zip;
            healthprofessional.Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            healthprofessional.Modifieddate = System.DateTime.Now;
            _context.Healthprofessionals.Update(healthprofessional);
            _context.SaveChanges();
        }

        public void deleteVendor(int venId)
        {
            Healthprofessional hp = _context.Healthprofessionals.FirstOrDefault(x => x.Vendorid == venId);
            hp.Isdeleted = new BitArray(new bool[] { true });
            _context.Healthprofessionals.Update(hp);
            _context.SaveChanges();
        }

        public List<Healthprofessionaltype> getAllHealthProfession()
        {
            var data = _context.Healthprofessionaltypes.ToList();
            return data;
        }

        public List<Healthprofessional> getHealthProfessionBussiness(int professionTypeId)
        {
            var data = _context.Healthprofessionals.Include(x => x.ProfessionNavigation).Where(x => professionTypeId == 0 || x.Profession == professionTypeId).Where(x => x.Isdeleted != new BitArray(new bool[] { true })).ToList();
            return data;
        }

        public Healthprofessional getVendorDetail(int vendorid)
        {
            return _context.Healthprofessionals.FirstOrDefault(x => vendorid == 0 || x.Vendorid == vendorid);
        }

        public void AddOrderDetail(int vendorid, int aspId, int reqid, string prescription, int refil)
        {
            Healthprofessional vendor = getVendorDetail(vendorid);
            Orderdetail order = new Orderdetail
            {
                Vendorid = vendorid,
                Requestid = reqid,
                Faxnumber = vendor.Faxnumber,
                Email = vendor.Email,
                Businesscontact = vendor.Businesscontact,
                Prescription = prescription,
                Noofrefill = refil,
                Createddate = DateTime.Now,
                Createdby = aspId.ToString(),
            };
            _context.Orderdetails.Add(order);
            _context.SaveChanges();
        }


        // Provider Location
        public IEnumerable<Physicianlocation> getAllPhysicianLocation()
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            return _context.Physicianlocations.ToList();
        }




        // admin profile
        public Admin getAdminProfile(string email)
        {
            var data = _context.Admins.Include(x => x.Aspnetuser).Include(x => x.Role).FirstOrDefault(x => x.Email == email);
            return data;
        }

        public AdminCustom getAdminCustomProfile(string email)
        {
            var admin = _context.Admins.FirstOrDefault(x => x.Email == email);
            if (admin != null)
            {
                return new AdminCustom
                {
                    Firstname = admin.Firstname,
                    Lastname = admin.Lastname,
                    Email = admin.Email,
                    Mobile = admin.Mobile
                };
            }
            else
            {
                return null;
            }
        }

        public List<Adminregion> getAdminReg(int adId)
        {
            return _context.Adminregions.Include(x => x.Region).Where(x => x.Adminid == adId).ToList();
        }

        public void updatePass(string email, string pass)
        {
            Aspnetuser asp = _context.Aspnetusers.FirstOrDefault(x => x.Email == email);
            asp.Passwordhash = Crypto.HashPassword(pass);
            asp.Modifieddate = System.DateTime.Now;
            _context.Aspnetusers.Update(asp);
            _context.SaveChanges();
        }

        public void updateAdminInfo(AdminCustom adminData, string email)
        {
            Admin admin = _context.Admins.FirstOrDefault(x => x.Email == email);
            admin.Firstname = adminData.Firstname;
            admin.Lastname = adminData.Lastname;
            admin.Mobile = adminData.Mobile;
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public void updateAdminLocation(Admin adminData, string email)
        {
            Admin admin = _context.Admins.FirstOrDefault(x => x.Email == email);
            admin.Address1 = adminData.Address1;
            admin.Address2 = adminData.Address2;
            admin.City = adminData.City;
            admin.Regionid = adminData.Regionid;
            admin.Zip = adminData.Zip;
            admin.Altphone = adminData.Altphone;
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public bool CheckAdminReg(int adminId, int Regid)
        {
            return _context.Adminregions.Any(x => x.Adminid == adminId && x.Regionid == Regid);
        }

        public void removeAdminReg(int adminId, int regionId)
        {
            Adminregion adreg = _context.Adminregions.FirstOrDefault(x => x.Adminid == adminId && x.Regionid == regionId);
            _context.Adminregions.Remove(adreg);
            _context.SaveChanges();
        }

        public void addAdminRegion(int adminId, int regioId)
        {
            Adminregion adreg = new Adminregion
            {
                Adminid = adminId,
                Regionid = regioId,
            };
            _context.Adminregions.Add(adreg);
            _context.SaveChanges();
        }


        //create admin
        public Aspnetuser CreteAdminAspnetUser(adminViewModel model)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Username = model.Lastname.Trim().ToUpper() + model.Firstname.Trim().ToString().Substring(0, 1).ToUpper(),
                Email = model.ConfirmEmail,
                Phonenumber = model.Phonenumber,
                Passwordhash = Crypto.HashPassword(model.Password),
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();

            return asp;
        }

        public Admin CreateNewAdmin(adminViewModel model, int cretedBy, int aspId)
        {
            Admin admin = new Admin
            {
                Aspnetuserid = aspId,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.ConfirmEmail,
                Mobile = model.Phonenumber,
                Address1 = model.Address1,
                Address2 = model.Address2,
                City = model.City,
                Regionid = model.regionId,
                Roleid = model.roleId,
                Zip = model.Zipcode,
                Altphone = model.AltPhonenumber,
                Createdby = cretedBy,
            };
            _context.Admins.Add(admin);
            _context.SaveChanges();
            return admin;
        }




        //Physician record

        public PhysicianCustom getPhyProfile(int phid)
        {
            var data = _context.Physicians.Where(r => r.Physicianid == phid)
                .Select(r => new PhysicianCustom
                {
                    Physicianid = phid,
                    Firstname = r.Firstname,
                    Lastname = r.Lastname,
                    Email = r.Email,
                    Mobile = r.Mobile,
                    Medicallicense = r.Medicallicense,
                    Npinumber = r.Npinumber,
                    Syncemailaddress = r.Syncemailaddress,
                }).FirstOrDefault();
            return data;
        }

        public List<Physician> getAllPhysicianData()
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            var data = _context.Physicians.Include(x => x.Physiciannotifications).Include(x => x.Role).Where(x => x.Isdeleted != bitArray).ToList();
            return data;
        }

        public string getPhysicianEmail(int phid)
        {
            return _context.Physicians.FirstOrDefault(x => x.Physicianid == phid).Email;
        }

        public string getPhysicianMobile(int phid)
        {
            return _context.Physicians.FirstOrDefault(x => x.Physicianid == phid).Mobile;

        }

        public Physician getPhysicianDetail(int phid)
        {
            return _context.Physicians.Include(x => x.Aspnetuser).Include(x => x.Role).FirstOrDefault(x => x.Physicianid == phid);
        }

        //role
        public List<Aspnetrole> getAllAspnetrole()
        {
            return _context.Aspnetroles.ToList();
        }


        // log history data
        public IEnumerable<Emaillog> getEmailLogData()
        {
            return _context.Emaillogs.Include(x => x.Request).ThenInclude(x => x.Requestclients).ToList();
        }
        public IEnumerable<Smslog> getSMSLogData()
        {
            return _context.Smslogs.Include(x => x.Request).ThenInclude(x => x.Requestclients).ToList();
        }

        public IEnumerable<Blockrequest> getallBlockRequest()
        {
            return _context.Blockrequests.Include(x => x.Request).ThenInclude(x => x.Requestclients).ToList();
        }


        //create role

        public List<Role> getAllroleDetails()
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            return _context.Roles.Where(x => x.Isdeleted != bitArray).ToList();
        }

        public Role AddRole(string roleName, short AccType, int aspId)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;
            Role role = new Role
            {
                Name = roleName,
                Accounttype = AccType,
                Createddate = DateTime.Now,
                Createdby = aspId,
                Isdeleted = bitArray,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Roles.Add(role);
            _context.SaveChanges();

            return role;
        }

        public void UpdateRole(int roleid, string roleName, short AccType, int aspId)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;
            Role role = _context.Roles.FirstOrDefault(x => x.Roleid == roleid);
            role.Name = roleName;
            role.Accounttype = AccType;
            role.Modifieddate = DateTime.Now;
            role.Modifiedby = aspId;
            role.Isdeleted = bitArray;
            role.Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            _context.Roles.Update(role);
            _context.SaveChanges();
        }


        public bool checkRoleMenu(int roleId, int menuId)
        {
            return _context.Rolemenus.Any(x => x.Roleid == roleId && x.Menuid == menuId);
        }

        public void addRoleMenu(int roleId, int menuId)
        {
            Rolemenu roleMenu = new Rolemenu
            {
                Roleid = roleId,
                Menuid = menuId,
            };
            _context.Rolemenus.Add(roleMenu);
            _context.SaveChanges();
        }

        public void removeRoleMenu(int roleId, int menuId)
        {
            Rolemenu rolemenu = _context.Rolemenus.FirstOrDefault(x => x.Roleid == roleId && x.Menuid == menuId);
            _context.Rolemenus.Remove(rolemenu);
            _context.SaveChanges();
        }


        //user Access

        public IEnumerable<UserAccessViewModel> userAccessData()
        {
            IEnumerable<UserAccessViewModel> data = _context.Aspnetusers.Include(x => x.Aspnetuserrole).Where(x => x.Aspnetuserrole.Roleid != 3).Include(x => x.PhysicianAspnetusers).Include(x => x.AdminAspnetusers).ToList()
                .Select(x => new UserAccessViewModel
                {
                    aspId = x.Id,
                    roleId = x.Aspnetuserrole.Roleid,
                    userName = x.Username,
                    Phone = x.Phonenumber,
                    status = (int)(x.Aspnetuserrole.Roleid == 1 ? x.AdminAspnetusers?.FirstOrDefault()?.Status : x.PhysicianAspnetusers.FirstOrDefault()?.Status),
                    totalRequest = x.Aspnetuserrole.Roleid == 2 ? _context.Requests.Where(i => i.Physicianid == x.PhysicianAspnetusers.FirstOrDefault().Physicianid).Count() : 0,
                });
            return data;
        }


        // All user Data
        public List<User> getAllUserData(string fname, string lname, string email, string phone)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            return _context.Users.Where(x =>
            (x.Isdeleted != new BitArray(new bool[] { true })) &&
            (fname == null || x.Firstname.Contains(fname)) &&
            (lname == null || x.Lastname.Contains(lname)) &&
            (email == null || x.Email.Contains(email)) &&
            (phone == null || x.Mobile.Contains(phone))
            ).ToList();
        }

        public List<Request> getAllReqData(string? reqStatus, int reqType)
        {
            var data = _context.Requests.Include(x => x.Requestclients).Include(x => x.Physician).Include(x => x.Requestnotes).Include(x => x.Requeststatuslogs)
                .Where(x => (x.Isdeleted != new BitArray(new bool[] { true })) &&
                (reqType == 0 || x.Requesttypeid.Equals(reqType))).ToList();
            if (reqStatus == "New")
            {
                data = data.Where(x => x.Status.Equals(1)).ToList();
            }
            if (reqStatus == "Pending")
            {
                data = data.Where(x => x.Status.Equals(2)).ToList();
            }
            if (reqStatus == "Active")
            {
                data = data.Where(x => x.Status.Equals(4) || x.Status.Equals(5)).ToList();
            }
            if (reqStatus == "Conclude")
            {
                data = data.Where(x => x.Status.Equals(6)).ToList();
            }
            if (reqStatus == "Toclose")
            {
                data = data.Where(x => x.Status.Equals(3) || x.Status.Equals(7) || x.Status.Equals(8)).ToList();
            }
            if (reqStatus == "Unpaid")
            {
                data = data.Where(x => x.Status.Equals(9)).ToList();
            }
            return data;
        }



        // shift schedule
        public Shift addNewShift(ShiftPoupViewModel model, string weekdays, int aspId)
        {
            BitArray isRepeatbit = new BitArray(1);
            isRepeatbit[0] = false;
            if (model.isRepeat == true)
            {
                isRepeatbit[0] = true;
            }

            Shift shift = new Shift
            {
                Physicianid = model.phyid,
                Startdate = model.shiftdate,
                Isrepeat = isRepeatbit,
                Weekdays = weekdays,
                Repeatupto = model.repeatTime,
                Createddate = System.DateTime.Now,
                Createdby = aspId,
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Shifts.Add(shift);
            _context.SaveChanges();
            return shift;
        }

        public bool IsDateRange(TimeOnly dbStartTime, TimeOnly dbEndTime, TimeOnly startTime, TimeOnly endTime)
        {
            //(dbStartTime <= startTime && dbEndTime >= startTime) || (dbStartTime <= endTime && dbEndTime >= endTime)
            return dbStartTime <= endTime && dbEndTime >= startTime;
        }

        public bool checkExistShift(ShiftPoupViewModel model, DateOnly newDate)
        {
            var filteredShifts = _context.Shiftdetails.Include(x => x.Shift)
                .Where(x => x.Shiftdate == newDate && x.Shift.Physicianid == model.phyid && x.Isdeleted != new BitArray(new bool[] { true }))
                .AsEnumerable()
                .Where(x => IsDateRange(x.Starttime, x.Endtime, model.timeStart, model.timeEnd));
            bool shiftExists = filteredShifts.Any();
            return shiftExists;
        }

        public Shiftdetail addNewShiftDetail(int shiftId, DateOnly shiftDate, ShiftPoupViewModel model, short status)
        {
            Shiftdetail shiftdetail = new Shiftdetail
            {
                Shiftid = shiftId,
                Shiftdate = shiftDate,
                Regionid = model.regId,
                Starttime = model.timeStart,
                Endtime = model.timeEnd,
                Status = status,
                Isdeleted = new BitArray(new bool[] { false }),
            };
            _context.Shiftdetails.Add(shiftdetail);
            _context.SaveChanges();
            return shiftdetail;
        }

        public void addShiftdetailRegion(int shiftDetailId, int regionId)
        {
            Shiftdetailregion shiftdetailregion = new Shiftdetailregion
            {
                Shiftdetailid = shiftDetailId,
                Regionid = regionId,
            };
            _context.Shiftdetailregions.Add(shiftdetailregion);
            _context.SaveChanges();
        }

        public List<phyCustomNameViewModel> getAllPhysicianName()
        {
            var listData = _context.Physicians.Include(x => x.Physicianregions)
               .Select(x => new phyCustomNameViewModel
               {
                   Firstname = x.Firstname,
                   Lastname = x.Lastname,
                   Physicianid = x.Physicianid,
                   photo = x.Photo,
                   phyReg = x.Physicianregions.Select(x => x.Regionid).ToList(),
               }).ToList();
            return listData;
        }

        public IEnumerable<Shiftdetail> getAllShiftdetail(int reg)
        {
            BitArray deleteBit = new BitArray(1);
            deleteBit[0] = true;
            return _context.Shiftdetails.Include(x => x.Shiftdetailregions).ThenInclude(x => x.Region).Include(x => x.Shift).ThenInclude(x => x.Physician)
                .Where(x => x.Isdeleted != deleteBit &&
                (reg == 0 || x.Shiftdetailregions.FirstOrDefault().Regionid == reg)).ToList();
        }


        public ShiftPoupViewModel getShiftDetailSpecific(int evenetId)
        {
            var data = _context.Shiftdetails
                .Include(x => x.Shift)
                .ThenInclude(x => x.Physician)
                .Where(x => x.Shiftdetailid == evenetId)
                .Select(x => new ShiftPoupViewModel
                {
                    shiftStatus = x.Status,
                    shiftdate = x.Shiftdate,
                    timeStart = x.Starttime,
                    timeEnd = x.Endtime,
                    phyid = x.Shift.Physicianid,
                    regId = (int)x.Regionid,
                })
                .FirstOrDefault();

            return data;
        }

        public void deleteShiftDetail(int shiftdetailId)
        {
            BitArray deleteBit = new BitArray(1);
            deleteBit[0] = true;
            Shiftdetail shiftdetail = _context.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftdetailId);
            shiftdetail.Isdeleted = deleteBit;
            _context.Shiftdetails.Update(shiftdetail);
            _context.SaveChanges();
        }

        public void changeShiftdetailStatus(int shiftdetailId, short status)
        {
            Shiftdetail shiftdetail = _context.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == shiftdetailId);
            shiftdetail.Status = status;
            _context.Shiftdetails.Update(shiftdetail);
            _context.SaveChanges();
        }

        public void updateShiftDetail(ShiftPoupViewModel model, int aspId)
        {
            Shiftdetail shiftdetail = _context.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == model.shiftDetailId);
            shiftdetail.Shiftdate = model.shiftdate;
            shiftdetail.Starttime = model.timeStart;
            shiftdetail.Endtime = model.timeEnd;
            shiftdetail.Modifieddate = System.DateTime.Now;
            shiftdetail.Modifiedby = aspId;
            _context.Shiftdetails.Update(shiftdetail);
            _context.SaveChanges();
        }


        public IEnumerable<ProcallModel> phyOncallAvialble(int reg)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeOnly currentTimeOnly = new TimeOnly(currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
            BitArray deleteBit = new BitArray(1);
            deleteBit[0] = true;
            var data = (from s in _context.Shiftdetails
                        join p in _context.Physicians on s.Shift.Physicianid equals p.Physicianid
                        join pr in _context.Physicianregions on p.Physicianid equals pr.Physicianid
                        where (reg == 0 || pr.Regionid == reg) &&
                        s.Shiftdate == DateOnly.FromDateTime(DateTime.Now) && (s.Starttime <= currentTimeOnly && s.Endtime > currentTimeOnly) && s.Isdeleted != deleteBit && p.Isdeleted != deleteBit
                        select new ProcallModel
                        {
                            phyId = p.Physicianid,
                            physicianFName = p.Firstname,
                            physicianLName = p.Lastname,
                            photo = p.Photo,
                        }).ToList();
            data = data.DistinctBy(x => x.phyId).ToList();
            return data;
        }

        public IEnumerable<ProcallModel> phyOffcall(int reg)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeOnly currentTimeOnly = new TimeOnly(currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
            BitArray deleteBit = new BitArray(1);
            deleteBit[0] = true;

            var data = (from physician in _context.Physicians
                        join physicianRegion in _context.Physicianregions on physician.Physicianid equals physicianRegion.Physicianid
                        where (reg == 0 || physicianRegion.Regionid == reg) &&
                              !_context.Shiftdetails.Any(x => x.Shift.Physicianid == physician.Physicianid &&
                                                                 x.Shiftdate == DateOnly.FromDateTime(DateTime.Now) &&
                                                                 (currentTimeOnly >= x.Starttime && currentTimeOnly <= x.Endtime) &&
                                                                 x.Isdeleted != deleteBit)
                        select new ProcallModel
                        {
                            phyId = physician.Physicianid,
                            physicianFName = physician.Firstname,
                            physicianLName = physician.Lastname,
                            photo = physician.Photo,
                        }).ToList();
            data = data.DistinctBy(x => x.phyId).ToList();
            return data;
        }


        public void removeBlockRequest(int reqId)
        {
            Blockrequest blockrequest = _context.Blockrequests.FirstOrDefault(x => x.Requestid == reqId);
            _context.Blockrequests.Remove(blockrequest);
            _context.SaveChanges();
        }












        //invoice
        public Providerpayrate providerPayrate(int phid)
        {
            Providerpayrate proPayRate = _context.Providerpayrates.FirstOrDefault(x => x.Physicianid == phid);
            return proPayRate;
        }

        public void proPayrateUpdate(int phid, int rate, string col)
        {
            Providerpayrate providerpayrate = _context.Providerpayrates.FirstOrDefault(x => x.Physicianid == phid);
            var property = typeof(Providerpayrate).GetProperty(col);

            if (property != null)
            {
                property.SetValue(providerpayrate, rate);
                _context.Providerpayrates.Update(providerpayrate);
                _context.SaveChanges();
            }
        }

        public List<Providerweeklysheet> getWeeksheetwithPhysician(int phid, int period, int month)
        {
            List<Providerweeklysheet> data = new List<Providerweeklysheet>();
            bool WeeksheetExist = _context.Providerfullsheets.Any(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month));
            if (WeeksheetExist)
            {
                int weeksheet = _context.Providerfullsheets.FirstOrDefault(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month)).Id;
                data = _context.Providerweeklysheets.Where(x => x.Sheetid == weeksheet).OrderBy(x => x.Weekdate).ToList();
            }
            return data;
        }


        public int addWeekSingleRecipt(int phid, int period, string startDate, string endDate)
        {
            DateOnly startedDates = DateOnly.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateOnly endedDates = DateOnly.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (!checkInvoiceFullexist(phid, period, startDate, endDate))
            {
                Providerfullsheet sheet = new Providerfullsheet()
                {
                    Peroid = Convert.ToInt16(period),
                    Physicianid = phid,
                    Startdate = startedDates,
                    Enddate = endedDates,
                    Finalize = false,
                };
                _context.Providerfullsheets.Add(sheet);
                _context.SaveChanges();

                return sheet.Id;
            }
            else
            {
                return _context.Providerfullsheets.FirstOrDefault(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Equals(startedDates) && x.Enddate.Equals(endedDates)).Id;
            }
        }

        public bool checkInvoiceFullexist(int phid, int period, string startDate, string endDate)
        {
            DateOnly startedDate = DateOnly.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateOnly Endeddate = DateOnly.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            return _context.Providerfullsheets.Any(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Equals(startedDate) && x.Enddate.Equals(Endeddate));
        }


        public void addsheetDataInvoice(List<InvoiceWeeklySheetData> weeklyData, int phid, int period)
        {
            int weektypesheet = addWeekSingleRecipt(phid, period, weeklyData.FirstOrDefault().date, weeklyData.LastOrDefault().date);

            foreach (var item in weeklyData)
            {
                DateOnly newDate = DateOnly.ParseExact(item.date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                bool exist = _context.Providerweeklysheets.Any(x => x.Weekdate.Value.Equals(newDate) && x.Sheetid == weektypesheet);
                if (!exist)
                {
                    Providerweeklysheet sheet = new Providerweeklysheet()
                    {
                        Housecall = item.housecall,
                        Consult = item.consult,
                        Totalhours = item.Totalhours,
                        Isholiday = item.isHoliday,
                        Weekdate = DateOnly.ParseExact(item.date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        Sheetid = weektypesheet,
                    };
                    _context.Providerweeklysheets.Add(sheet);
                }
                else
                {
                    Providerweeklysheet sheet = _context.Providerweeklysheets.FirstOrDefault(x => x.Weekdate.Equals(newDate) && x.Sheetid == weektypesheet);
                    sheet.Housecall = item.housecall;
                    sheet.Consult = item.consult;
                    sheet.Totalhours = item.Totalhours;
                    sheet.Isholiday = item.isHoliday;
                    _context.Providerweeklysheets.Update(sheet);
                }
            }
            _context.SaveChanges();
        }


        private string ConvertIFormFileToBase64String(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
        public void addreciptDataInvoice(List<ReciptWeeklySheet> recipteData, int phid, int period)
        {
            int weektypesheet = addWeekSingleRecipt(phid, period, recipteData.FirstOrDefault().date, recipteData.LastOrDefault().date);

            foreach (var item in recipteData)
            {
                DateOnly newDate = DateOnly.ParseExact(item.date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                bool exist = _context.Providerweeklysheets.Any(x => x.Weekdate.Value.Equals(newDate) && x.Sheetid == weektypesheet);
                if (!exist)
                {
                    Providerweeklysheet sheet = new Providerweeklysheet()
                    {
                        Sheetid = weektypesheet,
                        Weekdate = DateOnly.ParseExact(item.date, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                        Item = item.item,
                        Amount = item.amount,
                        Bill = ConvertIFormFileToBase64String(item.bill),
                    };
                    _context.Providerweeklysheets.Add(sheet);
                }
                else
                {
                    Providerweeklysheet sheet = _context.Providerweeklysheets.FirstOrDefault(x => x.Weekdate.Equals(newDate) && x.Sheetid == weektypesheet);
                    sheet.Item = item.item;
                    sheet.Amount = item.amount;
                    sheet.Bill = ConvertIFormFileToBase64String(item.bill);
                    _context.Providerweeklysheets.Update(sheet);
                }
            }
            _context.SaveChanges();
        }


        public void sheetFinalize(int period, int month, int phid)
        {
            bool WeeksheetExist = _context.Providerfullsheets.Any(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month));
            if (WeeksheetExist)
            {
                int weeksheet = _context.Providerfullsheets.FirstOrDefault(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month)).Id;
                Providerfullsheet fullSheet = _context.Providerfullsheets.FirstOrDefault(x => x.Id == weeksheet);
                fullSheet.Finalize = true;
                _context.Providerfullsheets.Update(fullSheet);
                _context.SaveChanges();
            }
        }

        public Providerfullsheet getFullSheetWithFinalize(int phid, int period, int month)
        {
            Providerfullsheet sheet = null;
            bool WeeksheetExist = _context.Providerfullsheets.Any(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month));
            if (WeeksheetExist)
            {
                sheet = _context.Providerfullsheets.FirstOrDefault(x => x.Physicianid == phid && x.Peroid == period && x.Startdate.Value.Month.Equals(month) && x.Finalize==true);
            }
            return sheet;
        }
    }
}
