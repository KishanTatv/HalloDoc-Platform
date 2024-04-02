using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
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


        #region Request Count
        public List<int> TotalCountPatient()
        {
            int Tnew = _context.Requests.Where(x => x.Status == 1).Count();
            int Tpending = _context.Requests.Where(x => x.Status == 2).Count();
            int Tactive = _context.Requests.Where(x => x.Status == 4 || x.Status == 5).Count();
            int Tconclide = _context.Requests.Where(x => x.Status == 6).Count();
            int Tclose = _context.Requests.Where(x => x.Status == 3 || x.Status == 7 || x.Status == 8).Count();
            int Tunpaid = _context.Requests.Where(x => x.Status == 9).Count();
            List<int> countList = new List<int>();
            countList.Add(Tnew); countList.Add(Tpending); countList.Add(Tactive); countList.Add(Tconclide); countList.Add(Tclose); countList.Add(Tunpaid);

            return countList;
        }
        #endregion

        #region AllRequest Data

        public DashTable GetPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype)
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
                           DateOfService = r.Modifieddate,
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

        public DashTable ExportPartialTableData(List<short>? status, int? page, int? pageSize, string? search, string? reg, int? reqtype)
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
                           DateOfService = r.Modifieddate,
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


        public void addNote(int reqid, string note)
        {
            Requestnote reqNote = new Requestnote()
            {
                Requestid = reqid,
                Adminnotes = note,
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


        public void AddBlockRequest(int reqId, string note)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;
            string email = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Email;
            string phoneNum = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Phonenumber;

            Blockrequest req = new Blockrequest()
            {
                Requestid = reqId,
                Reason = note,
                Email = email,
                Phonenumber = phoneNum,
                Isactive = bitArray,
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


        public List<Healthprofessionaltype> getAllHealthProfession()
        {
            var data = _context.Healthprofessionaltypes.ToList();
            return data;
        }

        public List<Healthprofessional> getHealthProfessionBussiness(int professionTypeId)
        {
            var data = _context.Healthprofessionals.Where(x => x.Profession == professionTypeId).ToList();
            return data;
        }

        public Healthprofessional getVendorDetail(int vendorid)
        {
            return _context.Healthprofessionals.FirstOrDefault(x => x.Vendorid == vendorid);
        }

        public void AddOrderDetail(int vendorid, int adminid, int reqid, string prescription, int refil)
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
                Createdby = adminid.ToString(),
            };
            _context.Orderdetails.Add(order);
            _context.SaveChanges();
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
            asp.Passwordhash = pass;
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
                Passwordhash = model.Password,
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
        public List<Aspnetuser> UserAccessData()
        {
            return _context.Aspnetusers.Include(x => x.Aspnetuserrole).Where(x => x.Aspnetuserrole.Roleid != 3).ToList();
        }



        // All user Data
        public List<User> getAllUserData()
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;
            return _context.Users.Where(x => x.Isdeleted != bitArray).ToList();
        }

        public List<Request> getAllReqData()
        {
            return _context.Requests.Include(x => x.Physician).ToList();
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

        public bool IsInDateRange(TimeOnly dateToCheck, TimeOnly startDate, TimeOnly endDate)
        {
            return dateToCheck >= startDate && dateToCheck <= endDate;
        }

        public bool checkExistShift(ShiftPoupViewModel model, DateOnly newDate)
        {
            return _context.Shiftdetails.Include(x => x.Shift).Where(x => x.Shiftdate == newDate && x.Shift.Physicianid == model.phyid && x.Starttime== model.timeStart).Any();
        }

        public Shiftdetail addNewShiftDetail(int shiftId, DateOnly shiftDate, ShiftPoupViewModel model, short status)
        {
            BitArray Deletebit = new BitArray(1);
            Deletebit[0] = false;
            Shiftdetail shiftdetail = new Shiftdetail
            {
                Shiftid = shiftId,
                Shiftdate = shiftDate,
                Regionid = model.regId,
                Starttime = model.timeStart,
                Endtime = model.timeEnd,
                Status = status,
                Isdeleted = Deletebit,
            };
            _context.Shiftdetails.Add(shiftdetail);
            _context.SaveChanges();
            return shiftdetail;
        }

        public List<phyCustomNameViewModel> getAllPhysicianName()
        {
            return _context.Physicians
               .Select(x => new phyCustomNameViewModel
               {
                   Firstname = x.Firstname,
                   Lastname = x.Lastname,
                   Physicianid = x.Physicianid,
               }).ToList();
        }

        public IEnumerable<Shiftdetail> getAllShiftdetail()
        {
            BitArray deleteBit = new BitArray(1);
            deleteBit[0] = true;
            return _context.Shiftdetails.Include(x => x.Shift).ThenInclude(x => x.Physician).Where(x => x.Isdeleted != deleteBit).ToList();
        }



        public ShiftPoupViewModel getShiftDetailSpecific(int evenetId)
        {
            var data = _context.Shiftdetails
                .Include(x => x.Shift)
                .ThenInclude(x => x.Physician)
                .Where(x => x.Shiftdetailid == evenetId)
                .Select(x => new ShiftPoupViewModel
                {
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

        public void updateShiftDetail(ShiftPoupViewModel model)
        {
            Shiftdetail shiftdetail = _context.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == model.shiftDetailId);
            shiftdetail.Shiftdate = model.shiftdate;
            shiftdetail.Starttime = model.timeStart;
            shiftdetail.Endtime = model.timeEnd;
            shiftdetail.Regionid = model.regId;
            _context.Shiftdetails.Update(shiftdetail);

            if (model.phyid != 0)
            {
                Shift shift = _context.Shifts.FirstOrDefault(x => x.Shiftid == _context.Shiftdetails.FirstOrDefault(x => x.Shiftdetailid == model.shiftDetailId).Shiftid);
                shift.Physicianid = model.phyid;
                _context.Shifts.Update(shift);
            }
            _context.SaveChanges();
        }
    }
}
