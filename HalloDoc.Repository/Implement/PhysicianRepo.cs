using GoogleMaps.LocationServices;
using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using HalloDoc.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

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


        public int getPhyId(string email)
        {
            return _context.Physicians.FirstOrDefault(x => x.Email == email).Physicianid;
        }

        public void acceptReqByPhysician(int reqid)
        {
            Request req = _context.Requests.FirstOrDefault(x => x.Requestid == reqid);
            req.Accepteddate = System.DateTime.Now;
            _context.Requests.Update(req);
            _context.SaveChanges();
        }


        public Aspnetuser CretephyAspnetUser(PhysicianCustom phy)
        {
            Aspnetuser asp = new Aspnetuser
            {
                Username = "MD." + phy.Lastname.ToUpper() + "." + phy.Firstname.ToString().Substring(0, 1).ToUpper(),
                Email = phy.Email,
                Phonenumber = phy.Mobile,
                Passwordhash = Crypto.HashPassword(phy.Password),
                Ip = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString(),
            };
            _context.Aspnetusers.Add(asp);
            _context.SaveChanges();

            return asp;
        }

        public void addPhyRegion(int phyId, int regionId)
        {
            Physicianregion phreg = new Physicianregion
            {
                Physicianid = phyId,
                Regionid = regionId,
            };
            _context.Physicianregions.Add(phreg);
            _context.SaveChanges();
        }

        public void removeRegion(int phyId, int regionId)
        {
            Physicianregion phreg = _context.Physicianregions.FirstOrDefault(x => x.Physicianid == phyId && regionId == regionId);
            phreg.Regionid = regionId;
            _context.Physicianregions.Remove(phreg);
            _context.SaveChanges();
        }

        public List<int> phyRegionExist(int phyId)
        {
            return _context.Physicianregions.Where(x => x.Physicianid == phyId).Select(x => x.Regionid).ToList();
        }

        public void addPhysicianNotification(int phyId)
        {
            BitArray bitArray = new BitArray(1);
            bitArray[0] = false;
            Physiciannotification phyNot = new Physiciannotification
            {
                Physicianid = phyId,
                Isnotificationstopped = bitArray,
            };
            _context.Physiciannotifications.Add(phyNot);
            _context.SaveChanges();
        }

        public void addPhysicianLocation(PhysicianProfileViewModel model, int phyId)
        {
            string Address = model.physician.Address1 + "," + model.physician.Address2 + "," + model.physician.City + "," + model.physician.Status + "," + model.physician.Zip;
            var locationService = new GoogleLocationService(apikey: "AIzaSyARrk6kY-nnnSpReeWotnQxCAo_MoI4qbU");
            var point = locationService.GetLatLongFromAddress(Address);
            decimal latitude = (decimal)point.Latitude;
            decimal longitude = (decimal)point.Longitude;

            Physicianlocation phyLoc = new Physicianlocation
            {
                Latitude = latitude,
                Longitude = longitude,
                Physicianid = phyId,
                Physicianname = model.PhysicianCustom.Firstname,
                Address = Address,
                Createddate = System.DateTime.Now,
            };
            _context.Physicianlocations.Add(phyLoc);
            _context.SaveChanges();
        }

        public void updatePhysicianNotification(int PhyId, BitArray isnotification)
        {
            Physiciannotification phyNot = _context.Physiciannotifications.FirstOrDefault(x => x.Physicianid == PhyId);
            phyNot.Isnotificationstopped = isnotification;
            _context.Physiciannotifications.Update(phyNot);
            _context.SaveChanges();
        }

        public Physician addNewPhysician(PhysicianProfileViewModel model, string photo, int aspId, int phyAspId)
        {
            Physician ph = new Physician
            {
                Aspnetuserid = phyAspId,
                Firstname = model.PhysicianCustom.Firstname,
                Lastname = model.PhysicianCustom.Lastname,
                Mobile = model.PhysicianCustom.Mobile,
                Medicallicense = model.PhysicianCustom.Medicallicense,
                Npinumber = model.PhysicianCustom.Npinumber,
                Email = model.PhysicianCustom.Email,
                Createddate = System.DateTime.Now,
                Createdby = aspId,
                Address1 = model.physician.Address1,
                Address2 = model.physician.Address2,
                Roleid = model.physician.Roleid,
                Status = 1,
                City = model.physician.City,
                Regionid = model.physician.Regionid,
                Zip = model.physician.Zip,
                Altphone = model.physician.Altphone,
                Businessname = model.physician.Businessname,
                Businesswebsite = model.physician.Businesswebsite,
                Photo = photo,
                Adminnotes = model.physician.Adminnotes,
            };
            _context.Physicians.Add(ph);
            _context.SaveChanges();

            return ph;
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

        public void updateAdditionPhyData(string busName, string busWeb, string photo, string sign, string AdminNote, string email, int aspId)
        {
            Physician phy = _context.Physicians.FirstOrDefault(x => x.Email == email);
            phy.Businessname = busName;
            phy.Businesswebsite = busWeb;
            phy.Photo = photo;
            phy.Signature = sign;
            phy.Adminnotes = AdminNote;
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

        public void AddOnboardingInphysician(int phyId, BitArray contractBit, BitArray backgrundBit, BitArray hipaaBit, BitArray discloureBit, BitArray licenceBit)
        {
            Physician phy = _context.Physicians.FirstOrDefault(x => x.Physicianid == phyId);
            phy.Isagreementdoc = contractBit;
            phy.Isbackgrounddoc = backgrundBit;
            phy.Istrainingdoc = hipaaBit;
            phy.Isnondisclosuredoc = discloureBit;
            phy.Islicensedoc = licenceBit;
            _context.Physicians.Update(phy);
            _context.SaveChanges();
        }
    }
}
