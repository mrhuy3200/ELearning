using ELearning_V2.common;
using ELearning_V2.DTO;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            ViewBag.domainName = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
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
                //var lstTopTeacher = db.NguoiDungs.OrderByDescending(x=>x.Courses.Select(c=>c.CourseDetails.Where(cd=>cd.CourseID == c.ID).Count())).Take(4).ToList();
                var lst = db.Courses.OrderByDescending(x => x.CourseDetails.Count()).Take(4).ToList();
                var temp = lst.GroupBy(x => x.NguoiDung).Select(grp => grp.OrderByDescending(x => x.ID).First()).ToList();

                List<TaiKhoanDTO> data = new List<TaiKhoanDTO>();
                foreach (var item in temp)
                {
                    TaiKhoanDTO t = new TaiKhoanDTO();
                    t.ID = item.ID;
                    t.Fullname = item.NguoiDung.HoVaTen;
                    t.Email = item.NguoiDung.Email;
                    t.Image = item.NguoiDung.Image;
                    t.Info = item.NguoiDung.Info;
                    data.Add(t);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetPublishLession()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lst = db.Course_Lession.Where(x => x.Status == 1).OrderByDescending(z => z.Lession.LessionViews.Count()).ThenByDescending(z=>z.Lession.Comments.Where(c=>c.CourseID == z.CourseID).Count()).Take(4).ToList();
                //var lstPublishLession = db.Lessions.Where(x => lst.Contains(x.ID)).OrderByDescending(o => o.LessionViews.Count()).Take(4).ToList();
                var temp = lst.GroupBy(x => x.LessionID).Select(grp => grp.OrderByDescending(x => x.Lession.Comments.Where(c => c.CourseID == x.CourseID).Count()).First()).ToList();
                List < LessionDTO > data = new List<LessionDTO>();
                foreach (var item in temp)
                {
                    LessionDTO l = new LessionDTO();
                    l.ID = item.Lession.ID;
                    l.Name = item.Lession.Name;
                    l.View = item.Lession.LessionViews.Count();
                    l.Content = item.Lession.Content;
                    l.CreateDate = (DateTime)item.Lession.CreateDate;
                    l.Image = item.Lession.Image;
                    l.CourseID = item.CourseID;
                    data.Add(l);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }
    }
}