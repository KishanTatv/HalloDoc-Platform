using HalloDoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository.Interface
{
    public interface IChat
    {
        int? getAspIdfromReqid(int reqid, string chatReqType);

        int getAspIsfromEmail(string email);
        string getchatPersonName(int aspId, string asptype);



        // chat entry

        void addMessageInChat(int senderId, int recverId, int reqid, string msg);

        IEnumerable<Chatmessage> getChatmessages(int senderId, int recverId, int reqid);
    }
}
