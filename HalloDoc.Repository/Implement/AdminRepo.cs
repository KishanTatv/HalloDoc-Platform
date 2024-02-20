using HalloDoc.Entity.AdminDashTable;
using HalloDoc.Entity.Data;
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

        public IEnumerable<tableData> GetTableData(int StatusId)
        {
            IEnumerable<tableData> data = from r in _context.Requests
                                          join f in _context.Requestclients on r.Requestid equals f.Requestid 
                                          where r.Status == StatusId
                                          orderby r.Createddate descending
                                          select new tableData
                                          {
                                              Name = f.Firstname + " " + f.Lastname,
                                              Dob = f.Dob,
                                              ReqTypeId = r.Requesttypeid,
                                              Requestor = r.Firstname + "," + r.Lastname,
                                              RequestedDate = r.Createddate,
                                              Phonenumber = f.Phonenumber,
                                              Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                                              Notes = r.Symptoms,
                                          };
            return data;
        }

        public IEnumerable<tableData> GetTableWithPhyData(int StatusId)
        {
            IEnumerable<tableData> data = from r in _context.Requests
                                          join f in _context.Requestclients on r.Requestid equals f.Requestid
                                          join s in _context.Physicians on r.Physicianid equals s.Physicianid
                                          where r.Status == StatusId
                                          orderby r.Createddate descending
                                          select new tableData
                                          {
                                              Name = f.Firstname + " " + f.Lastname,
                                              Dob = f.Dob,
                                              ReqTypeId = r.Requesttypeid,
                                              Requestor = r.Firstname + "," + r.Lastname,
                                              PhysicianName = s.Firstname + ", " + s.Lastname,
                                              DateOfService = s.Createddate,
                                              RequestedDate = r.Createddate,
                                              Phonenumber = f.Phonenumber,
                                              Address = f.Street + ", " + f.City + ", " + f.State + ", " + f.Zipcode,
                                              Notes = r.Symptoms,
                                          };
            return data;
        }

        public int TotalCountPatient(int statusId)
        {
            int Tnum = _context.Requests.Where(x => x.Status== statusId).Count();
            return Tnum;
        }
    }
}
