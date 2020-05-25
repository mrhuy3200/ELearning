using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ELearning_V2.DTO;
using ELearning_V2.Models;
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        private static List<TaiKhoanDTO> currentUser = new List<TaiKhoanDTO>();
        public void Send(string message)
        {
            var connectID = Context.ConnectionId;
            var connect = currentUser.Where(x => x.ConnectionID == connectID).FirstOrDefault();
            if (connect != null)
            {
                Clients.Group(connect.CourseID.ToString()).addChatMessage(connect.Fullname, message);
            }
        }
        public void SaveUser(string UserID)
        {
            var id = UserID;
        }
        public async Task JoinRoom(long UserID, long CourseID)
        {
            using (var db = new ELearningDB())
            {
                var user = db.NguoiDungs.Find(UserID);
                var courseDetail = db.CourseDetails.Where(x => x.CourseID == CourseID && x.UserID == UserID).FirstOrDefault();
                if (courseDetail != null)
                {
                    int index = currentUser.IndexOf( currentUser.Where(x => x.ConnectionID == Context.ConnectionId).FirstOrDefault());
                    currentUser[index].ID = UserID;
                    currentUser[index].Fullname = user.HoVaTen;
                    currentUser[index].CourseID = CourseID;

                    await Groups.Add(Context.ConnectionId, CourseID.ToString());
                    Clients.Group(CourseID.ToString()).addChatMessage(user.HoVaTen, " joined ");

                }
            }
        }
        public override Task OnConnected()
        {
            var UserID = long.Parse(Context.QueryString["UserID"]);
            var CourseID = long.Parse(Context.QueryString["CourseID"]);
            var a = Context.ConnectionId;
            var check = currentUser.Where(x=>x.ID == UserID && x.CourseID == CourseID).FirstOrDefault();
            if (check == null)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var us = db.NguoiDungs.Find(UserID);
                    var courseDetail = db.CourseDetails.Where(x => x.CourseID == CourseID && x.UserID == UserID).FirstOrDefault();
                    if (courseDetail != null)
                    {
                        TaiKhoanDTO user = new TaiKhoanDTO();
                        user.ConnectionID = a;
                        user.ID = UserID;
                        user.CourseID = CourseID;
                        user.Fullname = us.HoVaTen;
                        currentUser.Add(user);
                        Groups.Add(Context.ConnectionId, CourseID.ToString());
                        Clients.Group(CourseID.ToString()).addChatMessage(user.Fullname, " joined ");
                    }
                }
            }
            else
            {
                check.ConnectionID = a;
                Groups.Add(Context.ConnectionId, CourseID.ToString());
            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                var user = currentUser.Where(x=>x.ConnectionID == Context.ConnectionId).FirstOrDefault();
                Groups.Remove(Context.ConnectionId, user.CourseID.ToString());
                Clients.Group(user.CourseID.ToString()).addChatMessage(user.Fullname, " left ");
                currentUser.Remove(user);

                // We know that Stop() was called on the client,
                // and the connection shut down gracefully.
            }
            else
            {
                var user = currentUser.Where(x => x.ConnectionID == Context.ConnectionId).FirstOrDefault();
                Groups.Remove(Context.ConnectionId, user.CourseID.ToString());
                Clients.Group(user.CourseID.ToString()).addChatMessage(user.Fullname, " left ");
                currentUser.Remove(user);
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.
            }

            return base.OnDisconnected(stopCalled);
        }


    }
}