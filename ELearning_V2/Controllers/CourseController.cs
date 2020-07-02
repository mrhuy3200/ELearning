using ELearning_V2.DTO;
using ELearning_V2.Models;
using ELearning_V2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MyCourse()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public ActionResult GetClassByID(long ID)
        {
            var User = (TaiKhoan)Session["User"];

            using (ELearningDB db = new ELearningDB())
            {
                CourseDTO data = new CourseDTO();
                data = ClassService.GetClassByID(ID);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetMemberByClassID(long ID)
        {
            var User = (TaiKhoan)Session["User"];

            using (ELearningDB db = new ELearningDB())
            {
                List<TaiKhoanDTO> data = new List<TaiKhoanDTO>();
                data = ClassService.GetMemberByClassID(ID);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetLessionByClassID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            var data = ClassService.GetLessionByCourseID(ID);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetListTestByCourseID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            var data = ClassService.GetListTestByCourseID(ID);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetCommentByID(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            using (ELearningDB db = new ELearningDB())
            {
                if (c.CourseID != null)
                {
                    var data = ClassService.GetCommentByID(c, 2);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CheckJoinStatus(CourseDetailDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.CheckJoinStatus((long)c.CourseID, (long)c.UserID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserInfo()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.GetUserInfo(User.ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddMember(CourseDetailDTO cd)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var us = ClassService.GetUserInfo((long)cd.UserID);
            var course = ClassService.GetClassByID((long)cd.CourseID);
            if (course.Capacity == course.NumOfPeo)
            {
                //Lớp đã đầy
                return Json(2, JsonRequestBehavior.AllowGet);

            }
            if (us == null || course == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet);

            }
            CourseDetail data = new CourseDetail();
            data.UserID = us.ID;
            data.CourseID = course.ID;
            var checkJoin = ClassService.CheckJoinStatus(course.ID, us.ID);
            if (checkJoin == false)
            {
                if (ClassService.Pay(us.ID, (long)course.UserID, (double)course.Price))
                {
                    return Json(ClassService.AddMember(data), JsonRequestBehavior.AllowGet);
                }
                return Json(3, JsonRequestBehavior.AllowGet);

            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult LoadTestResult(TestResultDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Member = ClassService.CheckUserRole(User.ID, (long)t.CourseID) == 2 ? true : false;
            if (Member)
            {
                return Json(ClassService.GetTestResult((long)t.TestID, User.ID), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DoTest(long CourseID, long TestID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Member = ClassService.CheckUserRole(User.ID, CourseID) == 2 ? true : false;
            if (Member)
            {
                ViewBag.TestID = TestID;
                ViewBag.UserID = User.ID;
                ViewBag.CourseID = CourseID;
                return View("DoTest");
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult GetTestByID(TestResultDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Member = ClassService.CheckUserRole(User.ID, (long)t.CourseID) == 2 ? true : false;
            if (Member)
            {
                var data = ClassService.GetTestByID((long)t.TestID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTestResult(TestResultDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Member = ClassService.CheckUserRole(User.ID, (long)t.CourseID) == 2 ? true : false;
            if (Member)
            {
                var data = ClassService.GetTestResult((long)t.TestID, User.ID);
                if (data == null)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult SubmitTest(TestResultDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Member = ClassService.CheckUserRole(User.ID, (long)t.CourseID) == 2 ? true : false;
            if (Member && ClassService.GetTestResult((long)t.TestID, User.ID) == null)
            {
                return Json(ClassService.SubmitTest(t), JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMyCourse()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.GetClassByUserID(User.ID, 2), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExitCourse(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.ExitCourse(ID, User.ID), JsonRequestBehavior.AllowGet);

        }
        public ActionResult CountLessionView(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.CountLessionView(ID, User.ID), JsonRequestBehavior.AllowGet);

        }
    }
}