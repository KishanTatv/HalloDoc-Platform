using HalloDoc.Entity.AdminTab;
using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public interface IPhysician
    {
        void updatePhysicianInfo(PhysicianCustom physcian, string email, int aspId);
        void updatePhysicianLocation(Physician physician, string email, int aspId);
        void updateAdditionPhyData(string busName, string busWeb, string photo, string sign, string email, int aspId);
        void isDeletePhy(int phid);
    }
}
