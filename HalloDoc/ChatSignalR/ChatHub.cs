using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using HalloDoc.Entity.Models;
using HalloDoc.Repository.Interface;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Ocsp;
using System.Data;
using Twilio.TwiML.Messaging;

namespace HalloDoc.ChatSignalR
{
    public class ChatHub : Hub
    {
        private readonly IChat _Chat;
        public static Dictionary<string, string> ConnectionStore = new();


        public ChatHub(IChat Chat)
        {
            _Chat = Chat;
        }

        public async override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            string CookieEmail = httpContext!.Request.Cookies["CookieEmail"];
            string aspId = _Chat.getAspIsfromEmail(CookieEmail).ToString();
            if (string.IsNullOrEmpty(ConnectionStore.GetValueOrDefault(aspId)))
            {
                ConnectionStore.Add(aspId, Context.ConnectionId);
            }
            else
            {
                ConnectionStore.Remove(aspId);
                ConnectionStore.Add(aspId, Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }


        public async Task SendMessage(string user, string CookieEmail, string message, string reqid, string chatRecType)
        {
            var recAspId = _Chat.getAspIdfromReqid(Convert.ToInt32(reqid), chatRecType);
            int senAspId = _Chat.getAspIsfromEmail(CookieEmail);

            _Chat.addMessageInChat(senAspId, Convert.ToInt32(recAspId), Convert.ToInt32(reqid), message);

            var connectionid = ConnectionStore.GetValueOrDefault(recAspId.ToString());
            await Clients.Client(connectionid ?? "").SendAsync("ReceiveMessage", user, message);
        }
    }

}
