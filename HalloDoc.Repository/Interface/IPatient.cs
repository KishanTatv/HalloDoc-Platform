using HalloDoc.Entity.Models;
using HalloDoc.Entity.RequestForm;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public  interface IPatient
    {
        bool CheckExistAspUser(string email);


        int FindUserId(string email);


        //for patient
        Aspnetuser AddAspUser(ClientInformation clientInformation);
        User AddUser(ClientInformation clientInformation, int AspUserId);

        Request AddRequest(ClientInformation clientInformation, int userId);
        void AddRequestClient(ClientInformation clientInformation, int requestId);

        void AddDocFile(IFormFile DocFile, int reqId);


        //other form
        User AddFcbUser(FormFCB fcbInfo);
        Request AddFcbRequest(FormFCB fInfo, int UserId,int reqType);
        void AddFcbRequestClient(FormFCB fInfo, int requestId);
        Concierge AddConcierge(FormFCB fInfo);
        void AddRequestConcierge(FormFCB fInfo, int ReqId, int ConciergeId);

        Business AddBussiness(FormFCB fInfo);
        void AddRequestBussiness(FormFCB fInfo, int ReqId, int BussinessId);


        //create Patient
        Aspnetuser createonlyAsp(Aspnetuser user);
        void updateUserIdWithAsp(int aspId, string email);

        //User full name
        string userFullName(string email);

        //sendMail ResetPassword
        void sendMail(string email, string Sub, string bodyMsg);

        //New Password Create
        void newPasswordCreate(ClientInformation user, string email);




        //update profile patient

        void UpdateUser(PatientDash userInfo, string email);
        void UpdateRequestClient(PatientDash userInfo, string email);




        //Get User
        User GetUserByEmail(string email);
        Requestclient GetClientById(int id);

        User GetUserById(int id);

        IEnumerable<RequestWithFile> GetRequestsFiles(string email);

        IEnumerable<RequestWithFile> GetRequestsFileswithReq(string email, int reqId);
       

    }
}
