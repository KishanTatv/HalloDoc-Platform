using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Implement
{
    public class PhysicianRepo : IPhysician
    {
        private readonly HalloDocDbContext _context;
        private readonly IConfiguration _config;

        public PhysicianRepo(HalloDocDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public void updatePhysicianInfo(PhysicianCustom physcian, string email, int aspId)
        {
            Physician phy = _context.Physicians.FirstOrDefault(x => x.Email == email);
            phy.Firstname = physcian.Firstname;
            phy.Lastname = physcian.Lastname;
            phy.Mobile = physcian.Mobile;
            phy.Medicallicense = physcian.Medicallicense;
            phy.Npinumber = physcian.Npinumber;
            phy.Syncemailaddress = physcian.Syncemailaddress;
            phy.Modifieddate = System.DateTime.Now;
            phy.Modifiedby = aspId;
            _context.Physicians.Update(phy);
            _context.SaveChanges();
        }

        public void updatePhysicianLocation(Physician physician, string email, int aspId)
        {
            Physician phy = _context.Physicians.FirstOrDefault(x => x.Email == email);
            phy.Address1 = physician.Address1;
            phy.Address2 = physician.Address2;
            phy.City = physician.City;
            phy.Regionid = physician.Regionid;
            phy.Zip = physician.Zip;
            phy.Altphone = physician.Altphone;
            phy.Modifieddate = System.DateTime.Now;
            phy.Modifiedby = aspId;
            _context.Physicians.Update(phy);
            _context.SaveChanges();
        }

        public void updateAdditionPhyData(string busName, string busWeb, string photo,string sign, string email,int aspId)
        {
            Physician phy = _context.Physicians.FirstOrDefault(x => x.Email == email);
            phy.Businessname = busName;
            phy.Businesswebsite = busWeb;
            phy.Photo = photo;
            phy.Signature = sign;
            phy.Modifieddate = System.DateTime.Now;
            phy.Modifiedby = aspId;
            _context.Physicians.Update(phy);
            _context.SaveChanges();
        }

        public void isDeletePhy(int phid)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = true;

            Physician ph = _context.Physicians.FirstOrDefault(x => x.Physicianid == phid);
            ph.Isdeleted = bitArray;
            _context.Physicians.Update(ph);
            _context.SaveChanges();
        }
    }
}
