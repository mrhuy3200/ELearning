
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
        public bool Send(string message)
        {
            var connectID = Context.ConnectionId;
            var connect = currentUser.Where(x => x.ConnectionID == connectID).FirstOrDefault();
            if (connect != null)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Message data = new Message();
                    data.Content = message;
                    data.CreateDate = DateTime.Now;
                    data.CourseID = connect.CourseID;
                    data.UserID = connect.ID;
                    db.Messages.Add(data);
                    db.SaveChanges();
                    var msgID = db.Messages.OrderByDescending(x => x.ID).FirstOrDefault().ID;
                    Clients.Group(connect.CourseID.ToString(), Context.ConnectionId).addChatMessage(connect.Fullname, connect.ID, message, DateTime.Now.ToString("dd/MM HH:mm"), msgID);
                    Clients.Client(Context.ConnectionId).addChatMessageToMe(message, DateTime.Now.ToString("dd/MM HH:mm"), msgID);

                }
                return true;
            }
            return false;

        }

        public override Task OnConnected()
        {
            var UserID = long.Parse(Context.QueryString["UserID"]);
            var CourseID = long.Parse(Context.QueryString["CourseID"]);
            var a = Context.ConnectionId;
            var check = currentUser.Where(x => x.ID == UserID && x.CourseID == CourseID).FirstOrDefault();
            if (check == null)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var us = db.NguoiDungs.Find(UserID);
                    var courseDetail = db.CourseDetails.Where(x => x.CourseID == CourseID && x.UserID == UserID).FirstOrDefault();
                    var course = db.Courses.Find(CourseID);
                    if (courseDetail != null || course.UserID == UserID)
                    {
                        TaiKhoanDTO user = new TaiKhoanDTO();
                        user.ConnectionID = a;
                        user.ID = UserID;
                        user.CourseID = CourseID;
                        user.Fullname = us.HoVaTen;
                        currentUser.Add(user);
                        Groups.Add(Context.ConnectionId, CourseID.ToString());
                        //Clients.Group(CourseID.ToString()).addChatMessage(user.Fullname, " joined ");
                        var data = db.Messages.Where(x => x.CourseID == CourseID).OrderByDescending(x=>x.ID).Take(50).ToList();
                        var temp = data.OrderBy(x => x.ID).ToList();
                        foreach (var item in temp)
                        {
                            if (item.UserID == UserID)
                            {
                                Clients.Client(Context.ConnectionId).addChatMessageToMe(item.Content, item.CreateDate.Value.ToString("dd/MM HH:mm"), item.ID);
                            }
                            else
                            {
                                Clients.Client(Context.ConnectionId).addChatMessage(item.NguoiDung.HoVaTen, item.NguoiDung.ID, item.Content, item.CreateDate.Value.ToString("dd/MM HH:mm"), item.ID);
                            }
                        }
                    }
                }
            }
            else
            {
                check.ConnectionID = a;
                Groups.Add(Context.ConnectionId, CourseID.ToString());
                using (ELearningDB db = new ELearningDB())
                {
                    var data = db.Messages.Where(x => x.CourseID == CourseID);
                    foreach (var item in data)
                    {
                        if (item.UserID == UserID)
                        {
                            Clients.Client(Context.ConnectionId).addChatMessageToMe(item.Content, item.CreateDate.Value.ToString("dd/MM HH:mm"), item.ID);

                        }
                        else
                        {
                            Clients.Client(Context.ConnectionId).addChatMessage(item.NguoiDung.HoVaTen, item.NguoiDung.ID, item.Content, item.CreateDate.Value.ToString("dd/MM HH:mm"), item.ID);

                        }
                    }
                }


            }
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            if (stopCalled)
            {
                var user = currentUser.Where(x => x.ConnectionID == Context.ConnectionId).FirstOrDefault();
                Groups.Remove(Context.ConnectionId, user.CourseID.ToString());
                currentUser.Remove(user);

                // We know that Stop() was called on the client,
                // and the connection shut down gracefully.
            }
            else
            {
                var user = currentUser.Where(x => x.ConnectionID == Context.ConnectionId).FirstOrDefault();
                Groups.Remove(Context.ConnectionId, user.CourseID.ToString());
                currentUser.Remove(user);
                // This server hasn't heard from the client in the last ~35 seconds.
                // If SignalR is behind a load balancer with scaleout configured, 
                // the client may still be connected to another SignalR server.
            }

            return base.OnDisconnected(stopCalled);
        }


    }
}