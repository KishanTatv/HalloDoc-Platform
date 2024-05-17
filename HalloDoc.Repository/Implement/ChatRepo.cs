using HalloDoc.Entity.Data;
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Implement
{
    public class ChatRepo : IChat
    {
        private readonly HalloDocDbContext _context;

        public ChatRepo(HalloDocDbContext context)
        {
            _context = context;
        }

        public int? getAspIdfromReqid(int reqid, string chatReqType)
        {
            if (chatReqType == "Provider")
            {
                int? phid = _context.Requests.FirstOrDefault(x => x.Requestid == reqid).Physicianid;
                int? aspId = _context.Physicians.FirstOrDefault(x => x.Physicianid == phid).Aspnetuserid;
                return aspId;
            }
            else if (chatReqType == "Patient")
            {
                int? userid = _context.Requests.FirstOrDefault(x => x.Requestid == reqid).Userid;
                int? aspId = _context.Users.FirstOrDefault(x => x.Userid == userid).Aspnetuserid;
                return aspId;
            }
            else
            {
                //admin as reciver
                return 35;
            }
        }

        public int getAspIsfromEmail(string email)
        {
            return _context.Aspnetusers.FirstOrDefault(x => x.Email == email).Id;
        }



        public string getchatPersonName(int aspId, string asptype)
        {
            if (asptype == "Provider")
            {
                Physician phy = _context.Physicians.FirstOrDefault(x => x.Aspnetuserid == aspId);
                return phy.Firstname + " " + phy.Lastname;
            }
            else if(asptype == "Patient")
            {
                User user = _context.Users.FirstOrDefault(x => x.Aspnetuserid == aspId);
                return user.Firstname + " " + user.Lastname;
            }
            else
            {
                Admin admin = _context.Admins.FirstOrDefault(x => x.Aspnetuserid == aspId);
                return admin.Firstname + " " + admin.Lastname;
            }
        }





        // chat entry
        public void addMessageInChat(int senderId, int recverId, int reqid, string msg)
        {
            Chatmessage chatmessage = new Chatmessage()
            {
                Sender = senderId,
                Reciver = recverId,
                Requestid = reqid,
                Messages = msg,
                Createddate = DateTime.Now,
            };
            _context.Chatmessages.Add(chatmessage);
            _context.SaveChanges();
        }



        public IEnumerable<Chatmessage> getChatmessages(int senderId, int recverId, int reqid)
        {
            IEnumerable<Chatmessage> chatdata = _context.Chatmessages.Where(x =>
            (x.Sender == senderId || x.Sender == recverId) &&
            (x.Reciver == senderId || x.Reciver == recverId) &&
            x.Requestid == reqid).OrderBy(x => x.Createddate).ToList();
            return chatdata;
        }
    }
}
