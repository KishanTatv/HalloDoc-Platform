using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public  interface IPatient
    {
        bool CheckExistAspUser(string email);

        //for patient
        Aspnetuser AddAspUser(ClientInformation clientInformation);
        User AddUser(ClientInformation clientInformation, int AspUserId);

        Request AddRequest(ClientInformation clientInformation, int userId);
        void AddRequestClient(ClientInformation clientInformation, int requestId);

        Request AddOnlyRequest(ClientInformation clientInformation);
        void AddDocFile(IFormFile DocFile, int reqId);


        //other form
        Request AddOnlyFcbRequest(FormFCB fInfo, int reqType);
        User AddFcbUser(FormFCB fcbInfo);
        Request AddFcbRequest(FormFCB fInfo, int UserId,int reqType);
        void AddFcbRequestClient(FormFCB fInfo, int requestId);
        Concierge AddConcierge(FormFCB fInfo);
        void AddRequestConcierge(FormFCB fInfo, int ReqId, int ConciergeId);

        Business AddBussiness(FormFCB fInfo);
        void AddRequestBussiness(FormFCB fInfo, int ReqId, int BussinessId);
    }
}
