using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
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

        public Requestclient GetClientById(int id)
        {
            var userData = _context.Requestclients.FirstOrDefault(x => x.Requestclientid == id);
            return userData;
        }


        #region New Request
        public IEnumerable<tableData> GetTableData(int page, int pageSize)
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
                                              ReqClientId = f.Requestclientid,
                                              ReqTypeId = r.Requesttypeid,
                                              Requestor = r.Firstname + "," + r.Lastname,
                                              RequestedDate = r.Createddate,
                                              Phonenumber = f.Phonenumber,
                                              ReqPhonenumber = r.Phonenumber,
                                              Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                                              city = f.City,
                                              Notes = r.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Pending Request
        public IEnumerable<tableData> GetTableDataPending(int page, int pageSize)
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          join s in _context.Physicians on f.Physicianid equals s.Physicianid
                                          where f.Status == 2
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
                                              city = r.City,
                                              Notes = f.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Active Request
        public IEnumerable<tableData> GetTableDataActive(int page, int pageSize)
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
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              city = r.City,
                                              Notes = f.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Conclude Request
        public IEnumerable<tableData> GetTableDataConclude(int page, int pageSize)
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
                                              ReqClientId = r.Requestclientid,
                                              ReqTypeId = f.Requesttypeid,
                                              Requestor = f.Firstname + "," + f.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              city = r.City,
                                              Notes = f.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Toclose Request
        public IEnumerable<tableData> GetTableDataToclose(int page, int pageSize)
        {
            IEnumerable<tableData> data = from r in _context.Requestclients
                                          join f in _context.Requests on r.Requestid equals f.Requestid
                                          where f.Status == 3 || f.Status == 7 || f.Status == 8
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
                                              DateOfService = f.Accepteddate,
                                              RequestedDate = f.Createddate,
                                              Phonenumber = r.Phonenumber,
                                              ReqPhonenumber = f.Phonenumber,
                                              Address = r.Street + ", " + r.City + ", " + r.State + ", " + r.Zipcode,
                                              city = r.City,
                                              Notes = f.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion


        #region Unpaid Request
        public IEnumerable<tableData> GetTableDataUnpaid(int page, int pageSize)
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
                                              city = r.City,
                                              Notes = f.Symptoms,
                                          };
            return data.Skip(page * pageSize).Take(pageSize).ToList();
        }
        #endregion

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
    }
}
