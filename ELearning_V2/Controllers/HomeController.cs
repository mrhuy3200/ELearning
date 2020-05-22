using ELearning_V2.common;
using ELearning_V2.DTO;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult TrangChu()
        {
            var User = (TaiKhoan)Session["User"];
            if (TempData["ActiveResult"] != null)
            {
                ViewBag.ActiveResult = TempData["ActiveResult"].ToString();
            }
            if (TempData["RegisterResult"] != null)
            {
                ViewBag.ActiveResult = TempData["RegisterResult"].ToString();
            }
            if (TempData["Error"] != null)
            {
                ViewBag.ActiveResult = TempData["Error"].ToString();
            }
            return View();
        }
        public ActionResult GuessHomePage()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Chat()
        {
            return View();
        }
        public ActionResult GetFreeCourse()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var freeCourse = db.Courses.Where(x => x.Price == 0).OrderByDescending(x => x.Comments.Select(c => c.Rate).Sum() / x.Comments.Select(c => c.Rate).Count()).ToList();
                List<CourseDTO> data = new List<CourseDTO>();
                foreach (var item in freeCourse)
                {
                    var lstCmtID = db.Comments.Where(x => x.CourseID == item.ID).Select(a => a.ID);
                    var lstRep = db.Replies.Where(x => lstCmtID.Contains(x.CommentID)).ToList();

                    CourseDTO l = new CourseDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.Capacity = item.Capacity;
                    l.NumOfPeo = db.CourseDetails.Where(x => x.CourseID == item.ID).Count();
                    l.Description = item.Description;
                    l.Image = item.Image;
                    l.Status = item.Status;
                    l.Price = item.Price;
                    l.Schedule = item.Schedule;
                    l.Condition = item.Condition;
                    l.Type = item.Type;
                    l.UserID = item.UserID;
                    l.Comments = item.Comments.Count() + lstRep.Count();
                    l.Username = item.NguoiDung.HoVaTen;
                    data.Add(l);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }    
        public ActionResult GetTopTeacher()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstTopTeacher = db.NguoiDungs.OrderByDescending(x => x.Courses.Select(a => a.Comments.Select(c => c.Rate).Sum() / a.Comments.Select(c => c.Rate).Count()).Sum()).Take(4).ToList();
                List<TaiKhoanDTO> data = new List<TaiKhoanDTO>();
                foreach (var item in lstTopTeacher)
                {
                    TaiKhoanDTO t = new TaiKhoanDTO();
                    t.ID = item.ID;
                    t.Fullname = item.HoVaTen;
                    t.Email = item.Email;
                    t.Image = item.Image;
                    t.Info = item.Info;
                    data.Add(t);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPublishLession()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lst = db.Course_Lession.Where(x => x.Status == 1).Select(a => a.LessionID).ToList();
                var lstPublishLession = db.Lessions.Where(x => lst.Contains(x.ID)).OrderByDescending(o => o.LessionViews.Count()).Take(4).ToList();
                List<LessionDTO> data = new List<LessionDTO>();
                foreach (var item in lstPublishLession)
                {
                    LessionDTO l = new LessionDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.View = item.LessionViews.Count();
                    l.Content = item.Content;
                    l.CreateDate = (DateTime)item.CreateDate;
                    data.Add(l);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}