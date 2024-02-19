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

        public IEnumerable<tableData> GetTableData()
        {
            IEnumerable<tableData> data = from r in _context.Requests
                                          join f in _context.Requestclients on r.Requestid equals f.Requestid into rf
                                          from f in rf.DefaultIfEmpty()
                                          where r.Status == 1
                                          orderby r.Createddate descending
                                          select new tableData
                                          {
                                              Name = r.Firstname + " " + r.Lastname,
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
    }
}
