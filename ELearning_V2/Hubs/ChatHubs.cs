using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using ELearning_V2.DTO;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        private static List<TaiKhoanDTO> currentUser = new List<TaiKhoanDTO>();
        public void Send(string groupName, string name, string message)
        {
            Clients.Group(groupName).addChatMessage(name, message);

        }
        public async Task JoinRoom(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);
            Clients.Group(roomName).addChatMessage(Context.User.Identity.Name," joined "+roomName);
        }
        public override Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            var a = Context.ConnectionId;
            TaiKhoanDTO user = new TaiKhoanDTO();
            user.ConnectionID = a;
            currentUser.Add(user);
            return base.OnConnected();
        }
        public void SaveUser(long UserID)
        {
            var a = UserID;
        }    
    }
}