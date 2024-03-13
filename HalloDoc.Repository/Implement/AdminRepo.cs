using HalloDoc.Entity.AdminDash;
using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        #region New Request
        public IEnumerable<tableData> GetTableData()
        {
            IEnumerable<tableData> data = from r in _context.Requests
                                          join f in _context.Requestclients on r.Requestid equals f.Requestid
                                          where r.Status == 1 
                                          orderby r.Createddate descending
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
                                              Phonenumber = f.Phonenumber,
                                              ReqPhonenumber = r.Phonenumber,
                                              Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                                              Region = f.State,
                                              Notes = r.Symptoms,
                                          };
            return data;
        }
        #endregion


        #region Pending Request
        public IEnumerable<tableData> GetTableDataPending()
        {
            IEnumerable<tableData> data = from r in _context.Requests
                                          join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                                          join p in _context.Physicians on r.Physicianid equals p.Physicianid
                                          join l in _context.Requeststatuslogs on r.Requestid equals l.Requestid
                                          where l.Status == 2
                                          orderby r.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
                                              Intdate = rc.Intdate,
                                              Strmonth = rc.Strmonth,
                                              Intyear = rc.Intyear,
                                              Age = System.DateTime.Now.Year - rc.Intyear,
                                              RequestId = r.Requestid,
                                              ReqClientId = rc.Requestclientid,
                                              ReqTypeId = r.Requesttypeid,
                                              Requestor = r.Firstname + "," + r.Lastname,
                                              PhysicianName = p.Firstname + ", " + p.Lastname,
                                              DateOfService = r.Modifieddate,
                                              RequestedDate = r.Createddate,
                                              Phonenumber = rc.Phonenumber,
                                              ReqPhonenumber = rc.Phonenumber,
                                              Address = rc.Street + ", " + rc.City + ", " + rc.State + ", " + rc.Zipcode,
                                              Region = rc.State,
                                              Notes = l.Notes,
                                          };
            return data;
        }
        #endregion


        #region Active Request
        public IEnumerable<tableData> GetTableDataActive()
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          join s in _context.Physicians on f.Physicianid equals s.Physicianid
                                          where f.Status == 4 || f.Status == 5
                                          orderby f.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
                                              Intdate = r.Intdate,
                                              Strmonth = r.Strmonth,
                                              Intyear = r.Intyear,
                                              Age = System.DateTime.Now.Year - r.Intyear,
                                              RequestId = f.Requestid,
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              Region = r.State,
                                              Notes = f.Symptoms,
                                          };
            return data;
        }
        #endregion


        #region Conclude Request
        public IEnumerable<tableData> GetTableDataConclude()
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          join s in _context.Physicians on f.Physicianid equals s.Physicianid
                                          where f.Status == 6
                                          orderby f.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
                                              Intdate = r.Intdate,
                                              Strmonth = r.Strmonth,
                                              Intyear = r.Intyear,
                                              Age = System.DateTime.Now.Year - r.Intyear,
                                              RequestId = f.Requestid,
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              Region = r.State,
                                              Notes = f.Symptoms,
                                          };
            return data;
        }
        #endregion


        #region Toclose Request
        public IEnumerable<tableData> GetTableDataToclose()
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          join p in _context.Physicians on f.Physicianid equals p.Physicianid into preq
                                          from p in preq.DefaultIfEmpty()
                                          where f.Status == 3 || f.Status == 7 || f.Status == 8
                                          orderby f.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
                                              Intdate = r.Intdate,
                                              Strmonth = r.Strmonth,
                                              Intyear = r.Intyear,
                                              Age = System.DateTime.Now.Year - r.Intyear,
                                              RequestId = r.Requestid,
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              Region = r.State,
                                              Notes = f.Symptoms,
                                          };
            return data;
        }
        #endregion


        #region Unpaid Request
        public IEnumerable<tableData> GetTableDataUnpaid()
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          join s in _context.Physicians on f.Physicianid equals s.Physicianid
                                          where f.Status == 9
                                          orderby f.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
                                              Intdate = r.Intdate,
                                              Strmonth = r.Strmonth,
                                              Intyear = r.Intyear,
                                              Age = System.DateTime.Now.Year - r.Intyear,
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              Region = r.State,
                                              Notes = f.Symptoms,
                                          };
            return data;
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

        public List<Physician> GetAvaliablePhysician(int regionId)
        {
            var phyList = _context.Physicians.Where(x => x.Regionid == regionId).ToList();
            return phyList;
        }


        public void AddBlockRequest(int reqId, string note)
        {
            string email = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Email;
            string phoneNum = _context.Requestclients.FirstOrDefault(x => x.Requestid == reqId).Phonenumber;

            Blockrequest req = new Blockrequest()
            {
                Requestid = reqId.ToString(),
                Reason = note,
                Email = email,
                Phonenumber = phoneNum,
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
            var data = _context.Admins.Include(x => x.Aspnetuser).FirstOrDefault(x => x.Email == email);
            return data;
        }

        public void updatePass(string email, string pass)
        {
            Aspnetuser asp = _context.Aspnetusers.FirstOrDefault(x => x.Email == email);
            asp.Passwordhash = pass;
            _context.Aspnetusers.Update(asp);
            _context.SaveChanges();
        }

        public void updateAdminInfo(Admin adminData,string email)
        {
            Admin admin = _context.Admins.FirstOrDefault(x => x.Email == email);
            admin.Firstname = adminData.Firstname;
            admin.Lastname = adminData.Lastname;
            admin.Mobile = adminData.Mobile;
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

        public void updateAdminLocation(Admin adminData,string email)
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
    }
}
