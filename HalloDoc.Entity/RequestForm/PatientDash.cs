using System.ComponentModel.DataAnnotations;
using HalloDoc.Entity.Models;

namespace HalloDoc.Entity.RequestForm
{
    public class PatientDash
    {
        //public IEnumerable<Request> Requests { get; set; }
        public User User { get; set; }

        public IEnumerable<RequestWithFile> ReqWithFiles { get; set; }

        public Requestclient reqclient { get; set; }    

        public ClientInformation clientInfo { get; set; }

    }
}
