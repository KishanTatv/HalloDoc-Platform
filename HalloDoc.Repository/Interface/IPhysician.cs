using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public interface IPhysician
    {
        Aspnetuser CretephyAspnetUser(ClientInformation user, PhysicianCustom phy);
        Physician addNewPhysician(PhysicianProfileViewModel model, string photo, int aspId, int phyAspId);

        void addPhyRegion(int phyId, int regioId);
        void removeRegion(int phyId, int regionId);
        List<int> phyRegionExist(int phyId);

        void addPhysicianNotification(int phyId);
        void updatePhysicianNotification(int PhyId, BitArray isnotification);

        void updatePhysicianInfo(PhysicianCustom physcian, string email, int aspId);
        void updatePhysicianLocation(Physician physician, string email, int aspId);
        void updateAdditionPhyData(string busName, string busWeb, string photo, string sign, string adminNote, string email, int aspId);
        void isDeletePhy(int phid);
    }
}
