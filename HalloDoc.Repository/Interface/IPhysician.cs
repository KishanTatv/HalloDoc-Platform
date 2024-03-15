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
        void updatePhysicianInfo(Physician physcian, string email, int aspId);
        void updatePhysicianLocation(Physician physician, string email, int aspId);
        void updateAdditionPhyData(Physician physician, string email, int aspId);
    }
}
