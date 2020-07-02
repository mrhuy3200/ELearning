using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.DTO;
using ELearning_V2.Models;
using ELearning_V2.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

namespace ELearning_V2.Controllers
{
    public class LopController : Controller
    {
        // GET: Lop
        public ActionResult DanhSachLopHoc()
        {
            //var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                var Lops = db.Lops.Where(x => x.TrangThai == true);
                List<LopModel> lstLop = new List<LopModel>();
                foreach (var item in Lops)
                {
                    LopModel lop = new LopModel();
                    lop.TenLop = item.TenLop;
                    lop.HoTenGV = item.GiangVien.HoVaTen;
                    lop.NgayBatDau = item.NgayBatDau.Value.ToString("dd/MM/yyyy");
                    lop.SiSo = item.SiSo;
                    lop.TenMonHoc = item.MonHoc.TenMonHoc;
                    lop.Image = item.Image;
                    lop.MoTa = item.MoTa;
                    lop.MaLop = item.MaLop;
                    lstLop.Add(lop);
                }
                return View(lstLop);
            }
        }

        [HttpGet]
        public ActionResult MyClass()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                //FlashMessage.Warning("Bạn chưa đăng nhập!!!");
                TempData["Error"] = "Bạn chưa đăng nhập";
                return RedirectToAction("TrangChu", "Home");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var lstLop = db.Lops.Where(x => x.HocViens.Any(c => c.ID == session.ID)).ToList();
                List<LopModel> Lops = new List<LopModel>();
                foreach (var item in lstLop)
                {
                    LopModel lop = new LopModel();
                    lop.TenLop = item.TenLop;
                    lop.HoTenGV = item.GiangVien.HoVaTen;
                    lop.NgayBatDau = item.NgayBatDau.Value.ToString("dd/MM/yyyy");
                    lop.SiSo = item.SiSo;
                    lop.TenMonHoc = item.MonHoc.TenMonHoc;
                    lop.Image = item.Image;
                    lop.MoTa = item.MoTa;
                    lop.MaLop = item.MaLop;
                    Lops.Add(lop);
                }
                return View(Lops);
            }
        }

        [HttpGet]
        public ActionResult ViewDetails(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop l = db.Lops.Where(x => x.MaLop == id && x.TrangThai == true).FirstOrDefault();
                LopModel lop = new LopModel();
                lop.MaLop = l.MaLop;
                lop.TenLop = l.TenLop;
                lop.MaGiangVien = (long)l.MaGiangVien;
                lop.HoTenGV = l.GiangVien.HoVaTen;
                lop.MoTa = l.MoTa;
                lop.NgayBatDau = l.NgayBatDau.Value.ToString("dd/MM/yyyy");
                lop.TenMonHoc = l.MonHoc.TenMonHoc;
                lop.SiSo = l.SiSo;

                return View(lop);
            }
        }

        [HttpGet]
        public ActionResult DangKy(int id)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                if (session == null)
                {
                    TempData["Error"] = "Bạn chưa đăng nhập";
                    return RedirectToAction("ViewDetails", "Lop", new { id = id });
                }
                else
                {
                    if (session.loai != 3)
                    {
                        TempData["Error"] = "Chức năng này dành cho học viên";
                        return RedirectToAction("ViewDetails", "Lop", new { id = id });
                    }
                }
                HocVien hv = db.HocViens.Find(session.ID);
                Lop l = db.Lops.Find(id);
                int res = l.HocViens.Where(x => x.ID == session.ID).Count();
                if (res != 0)
                {
                    TempData["Error"] = "Lớp học đã đăng ký trước đó";
                    return RedirectToAction("ViewDetails", "Lop", new { id = id });
                }
                l.HocViens.Add(hv);
                db.SaveChanges();
                return RedirectToAction("MyClass", "Lop");
            }
        }

        [HttpGet]
        public ActionResult ClassDetails(int id)
        {
            ViewBag.LopID = id;
            return View();
        }

        [HttpPost]
        public ActionResult DanhSachLopHoc(string key)
        {
            using (ELearningDB db = new ELearningDB())
            {
                List<Lop> lstLop = db.Lops.Where(x => x.TenLop.Contains(key) || x.GiangVien.HoVaTen.Contains(key) && x.TrangThai == true).ToList();
                List<LopModel> Lops = new List<LopModel>();
                foreach (var item in lstLop)
                {
                    LopModel lop = new LopModel();
                    lop.TenLop = item.TenLop;
                    lop.HoTenGV = item.GiangVien.HoVaTen;
                    lop.NgayBatDau = item.NgayBatDau.Value.ToString("dd/MM/yyyy");
                    lop.SiSo = item.SiSo;
                    lop.TenMonHoc = item.MonHoc.TenMonHoc;
                    lop.Image = item.Image;
                    lop.MoTa = item.MoTa;
                    lop.MaLop = item.MaLop;
                    Lops.Add(lop);
                }
                return View(Lops);
            }
        }

        public ActionResult TimKiem()
        {
            List<Lop> Lops = TempData["Lops"] as List<Lop>;
            return View(Lops);
        }

        [HttpGet]
        public JsonResult Details(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop l = db.Lops.Find(id);
                LopModel Lop = new LopModel();
                Lop.MaLop = l.MaLop;
                Lop.TenLop = l.TenLop;
                Lop.TrangThai = l.TrangThai;
                Lop.NgayBatDau = l.NgayBatDau.ToString(); ;
                Lop.HoTenGV = l.GiangVien.HoVaTen;
                Lop.TenMonHoc = l.MonHoc.TenMonHoc;
                Lop.MoTa = l.MoTa;
                Lop.SiSo = l.SiSo;

                return Json(Lop, JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetThongBao_TheoLopID(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstTB = db.ThongBaos.Where(x => x.LopID == id);
                List<ThongBaoModel> ThongBaos = new List<ThongBaoModel>();
                foreach (var item in lstTB)
                {
                    ThongBaoModel thongbao = new ThongBaoModel();
                    thongbao.ID = item.ID;
                    thongbao.NoiDung = item.NoiDung;
                    thongbao.NgayTao = item.NgayTao.Value.ToString("dd/MM/yyyy");
                    ThongBaos.Add(thongbao);
                }
                return Json(ThongBaos, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DayThem()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public ActionResult GetClassByUserID()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                List<CourseDTO> data = new List<CourseDTO>();
                data = ClassService.GetClassByUserID(User.ID, 1);
                return Json(data, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult RemoveCourse(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var check = db.Courses.Find(ID);
                if (check == null)
                {
                    return Json("Không tìm thấy lớp", JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                var NotiID = db.Notifications.Where(a=>a.CourseID == ID).Select(x => x.ID).ToList();
                foreach (var item in NotiID)
                {
                    NotificationDTO n = new NotificationDTO() { ID=item};
                    RemoveNotification(n);
                }
                int res = ClassService.RemoveCourse(check);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetClassByID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                CourseDTO data = new CourseDTO();
                data = ClassService.GetClassByID(ID);
                if (data.UserID != User.ID)
                {
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMemberByClassID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                List<TaiKhoanDTO> data = new List<TaiKhoanDTO>();
                data = ClassService.GetMemberByClassID(ID);
                if (db.Courses.Find(ID).UserID != User.ID)
                {
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetLessionByUserID()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetLessionByUserID(User.ID);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetLessionByClassID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            using (ELearningDB db = new ELearningDB())
            {
                var Course = db.Courses.Find(ID);

                var data = ClassService.GetLessionByCourseID(ID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult AddLessionToCourse(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var Course = db.Courses.Find(l.CourseID);
                var lession = db.Lessions.Find(l.ID);
                var course_lession = db.Course_Lession.Where(x => x.CourseID == l.CourseID && x.LessionID == l.ID).FirstOrDefault();
                if (Course == null)
                {
                    return Json("Không tìm thấy lớp", JsonRequestBehavior.AllowGet);
                }
                if (lession == null)
                {
                    return Json("Không tìm thấy bài giảng", JsonRequestBehavior.AllowGet);
                }
                if (Course.UserID != User.ID || lession.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                if (course_lession != null)
                {
                    return Json("Bài giảng đã thêm trước đó", JsonRequestBehavior.AllowGet);
                }
                var data = ClassService.AddLessionToCourse((long)l.CourseID, l.ID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult RemoveLessionFromCourse(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var course_lession = db.Course_Lession.Where(x => x.CourseID == l.CourseID && x.LessionID == l.ID).FirstOrDefault();
                if (course_lession == null)
                {
                    return Json("Bài giảng chưa được gán cho lớp này", JsonRequestBehavior.AllowGet);
                }
                if (course_lession.Course.UserID != User.ID || course_lession.Lession.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                var data = ClassService.RemoveLessionFromCourse((long)l.CourseID, l.ID);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoadLessionToAdd(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var Course = db.Courses.Find(ID);
                if (Course.UserID == User.ID)
                {
                    var data = ClassService.LoadLessionToAdd(ID, User.ID);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreateClass(Course c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (c.Type != 1)
            {
                var pay = ClassService.Pay(User.ID, 1, c.Type == 2 ? 200000 : 500000);
                if (pay)
                {
                    using (ELearningDB db = new ELearningDB())
                    {
                        Course co = new Course();
                        co.Name = c.Name;
                        if (c.Type == 2)
                        {
                            c.Capacity = 45;
                        }
                        else
                        {
                            c.Capacity = null;
                        }
                        c.UserID = User.ID;
                        c.Status = 1;
                        db.Courses.Add(c);
                        db.SaveChanges();
                        long id = db.Courses.OrderByDescending(p => p.ID).FirstOrDefault().ID;
                        c.Image = id + ".jpg";
                        db.SaveChanges();
                        return Json(id, JsonRequestBehavior.AllowGet);
                    }

                }
                return Json(-1, JsonRequestBehavior.AllowGet);

            }
            using (ELearningDB db = new ELearningDB())
            {
                Course co = new Course();
                co.Name = c.Name;
                co.Capacity = 15;
                co.Description = c.Description;
                co.Price = 0;
                co.Schedule = c.Schedule;
                co.Condition = c.Condition;
                co.Type = c.Type;
                co.UserID = User.ID;
                c.UserID = User.ID;
                c.Status = 1;
                db.Courses.Add(c);
                db.SaveChanges();
                long id = db.Courses.OrderByDescending(p => p.ID).FirstOrDefault().ID;
                c.Image = id + ".jpg";
                db.SaveChanges();
                return Json(id, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult EditCourse(CourseDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (c.UserID != User.ID)
            {
                return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
            }
            if (ClassService.EditCourse(c))
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddMember(CourseDetailDTO cd)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var u = db.TaiKhoans.Where(x => x.Username == cd.Username).FirstOrDefault();
                var c = db.Courses.Find(cd.CourseID);
                if (u == null)
                {
                    return Json("Không tìm thấy người dùng", JsonRequestBehavior.AllowGet);
                }
                if (c == null)
                {
                    return Json("Không tìm lớp học", JsonRequestBehavior.AllowGet);
                }
                if (c.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                CourseDetail cou = new CourseDetail();
                cou.CourseID = c.ID;
                cou.UserID = u.ID;
                int res = ClassService.AddMember(cou);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                if (res == -1)
                {
                    return Json("Thêm thất bại, người dùng đã tham gia lớp trước đó", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RemoveMember(CourseDetailDTO cd)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var u = db.TaiKhoans.Find(cd.UserID);
                var c = db.Courses.Find(cd.CourseID);
                if (u == null)
                {
                    return Json("Không tìm thấy người dùng", JsonRequestBehavior.AllowGet);
                }
                if (c == null)
                {
                    return Json("Không tìm lớp học", JsonRequestBehavior.AllowGet);
                }
                if (c.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                CourseDetail cou = new CourseDetail();
                cou.CourseID = c.ID;
                cou.UserID = u.ID;
                int res = ClassService.RemoveMember(cou);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                if (res == -1)
                {
                    return Json("Xóa thất bại, người dùng không thuộc lớp học", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ChangeLessionStatus(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                if (db.Lessions.Find(l.ID).UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                int res = ClassService.ChangeStatusLession(l);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ChangeCourseStatus(CourseDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                if (db.Courses.Find(c.ID).UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                int res = ClassService.ChangeStatusCourse(c);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateLession()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            LessionDTO l = new LessionDTO();
            l.UserID = User.ID;
            return View(l);
        }

        public ActionResult EditLession(long ID, long? CourseID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var check = db.Lessions.Find(ID);
                if (check == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                LessionDTO l = new LessionDTO();
                l = ClassService.GetLessionByID(ID, CourseID);
                return View("EditLession", l);
            }
        }
        [HttpPost]
        public ActionResult CreateLession(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                long res = ClassService.CreateLession(l);
                if (res != 0)
                {
                    return Json(res, JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditLession(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var checkLession = db.Lessions.Find(l.ID);
                if (checkLession == null)
                {
                    return Json("Không tìm thấy bài giảng", JsonRequestBehavior.AllowGet);
                }
                if (checkLession.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                long res = ClassService.EditLession(l);
                if (res != 0)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult RemoveLession(LessionDTO l)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                if (db.Lessions.Find(l.ID).UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                int res = ClassService.RemoveLession(l);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CourseDetail(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                Course c = new Course();
                c = db.Courses.Find(ID);
                if (c == null)
                {
                    return Json("Không tìm thấy lớp học", JsonRequestBehavior.AllowGet);

                }
                if (c.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                return View(c);
            }
        }
        public ActionResult ViewCourse(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                ViewBag.UserID = 0;
            }
            else
            {
                ViewBag.UserID = User.ID;
            }
            ViewBag.CourseID = ID;
            return View();
        }
        public ActionResult LessionDetail(long ID, long? CourseID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                ViewBag.UserID = 0;
            }
            else
            {
                ViewBag.UserID = User.ID;
            }

            ViewBag.LessionID = ID;
            ViewBag.CourseID = CourseID;
            return View();
        }

        public ActionResult GetLessionByID(long ID, long? CourseID)
        {
            var User = (TaiKhoan)Session["User"];

            using (ELearningDB db = new ELearningDB())
            {
                LessionDTO l = new LessionDTO();
                l = ClassService.GetLessionByID(ID, CourseID);
                //if (CourseID == -1)
                //{
                //    l = ClassService.GetLessionByID(ID, null);

                //}
                //else
                //{
                //    l = ClassService.GetLessionByID(ID, CourseID);

                //}
                if (User == null)
                {
                    if (l.Course_LessionStatus == 1)
                    {
                        return Json(l, JsonRequestBehavior.AllowGet);
                    }
                    return Json(null, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    if (l.Course_LessionStatus == 1)
                    {
                        return Json(l, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (l.UserID == User.ID)
                        {
                            return Json(l, JsonRequestBehavior.AllowGet);

                        }
                        if (ClassService.CheckUserRole(User.ID, (long)l.CourseID) == 1 || ClassService.CheckUserRole(User.ID, (long)l.CourseID) == 2)
                        {
                            return Json(l, JsonRequestBehavior.AllowGet);
                        }
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult GetCommentByID(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            using (ELearningDB db = new ELearningDB())
            {
                if (c.LessionID != null)
                {
                    if (c.CourseID != null)
                    {
                        var course_lession = db.Course_Lession.Where(x => x.CourseID == c.CourseID && x.LessionID == c.LessionID).FirstOrDefault();
                        if (course_lession.Status == 1)
                        {
                            var data = ClassService.GetCommentByID(c, 3);
                            return Json(data, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            if (User == null)
                            {
                                return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);

                            }
                            var owner = db.Lessions.Find(c.LessionID).UserID == User.ID ? true : false;
                            var member = db.CourseDetails.Where(x => x.CourseID == c.CourseID && x.UserID == User.ID).FirstOrDefault() == null ? false : true;
                            if (owner || member)
                            {
                                var data = ClassService.GetCommentByID(c, 3);
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                            return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);

                        }

                    }
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                if (c.CourseID != null)
                {
                    var check = db.Courses.Find(c.CourseID);
                    if (check.UserID == User.ID)
                    {
                        var data = ClassService.GetCommentByID(c, 2);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                if (c.ClassID != null)
                {
                    var check = db.Lops.Find(c.ClassID);
                    if (check.MaGiangVien == User.ID)
                    {
                        var data = ClassService.GetCommentByID(c, 1);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult CreateReply(ReplyDTO r)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            r.CreateBy = User.ID;
            var Data = ClassService.CreateReply(r);
            if (Data == 1)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateComment(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            c.CreateBy = User.ID;
            var Data = ClassService.CreateComment(c);
            if (Data == 1)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditComment(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.EditComment(c))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveComment(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.RemoveComment(c.ID))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditReply(ReplyDTO r)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.EditReply(r))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveReply(ReplyDTO r)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.RemoveReply(r))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListQuesionByUserID()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetListQuestionByUserID(User.ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListQuesionByTestID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetListQuestionByTestID(ID, User.ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RandomTestQuestion(CauHoiDeThiModel c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.GetTestByID(c.DeThiID).UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            var data = ClassService.RandomTestQuestion(c.DeThiID, User.ID, c.SoCauHoi, c.DoKho, c.ChuongID);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ChangeQuestionStatus(QuestionDTO q)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                if (db.Questions.Find(q.ID).UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
                int res = ClassService.ChangeStatusQuestion(q);
                if (res == 1)
                {
                    return Json("OK", JsonRequestBehavior.AllowGet);
                }
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateQuestion()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
        public ActionResult GetTopic()
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetListTopicByUserID(User.ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateQuestion(QuestionDTO d)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            Question q = new Question();
            q.Content = d.Content;
            q.Level = d.Level;
            q.Solution = d.Solution;
            q.CreateDate = DateTime.Now;
            q.UserID = User.ID;
            long QuestionID = ClassService.CreateQuestion(q);
            if (d.Topics != null)
            {
                ClassService.CreateQuestionTopic(QuestionID, d.Topics);
            }
            if (QuestionID != 0)
            {
                int i = 0;
                foreach (var item in d.Answers)
                {
                    Answer a = new Answer();
                    a.Content = item.Content;
                    a.QuesionID = QuestionID;
                    long AnswerID = ClassService.CreateAnswer(a);
                    if (d.AnswerID == i)
                    {
                        using (ELearningDB db = new ELearningDB())
                        {
                            var data = db.Questions.Find(QuestionID);
                            data.AnswerID = AnswerID;
                            db.SaveChanges();
                        }
                    }
                    i++;
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditQuestion(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var data = db.Questions.Find(ID);
                if (data == null)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                if (data.UserID == User.ID)
                {
                    ViewBag.QuestionID = data.ID;
                    return View();
                }
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EditQuestion(QuestionDTO q)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (q.UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (ClassService.EditQuestion(q))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult RemoveQuestion(QuestionDTO q)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (q.UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            if (ClassService.RemoveQuestion(q))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetQuestion(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetQuestion(ID);
            if (data.UserID == User.ID)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateTest(TestDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.CreateTest(t), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditTest(TestDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.EditTest(t), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteTest(TestDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.DeleteTest(t), JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangeTestStatus(TestDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.ChangeTestStatus(t), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListTestByCourseID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var Course = ClassService.GetClassByID(ID);
            using (ELearningDB db = new ELearningDB())
            {
                if (Course.UserID != User.ID || db.CourseDetails.Where(x => x.CourseID == ID && x.UserID == User.ID) == null)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
            }
            var data = ClassService.GetListTestByCourseID(ID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestDetail(long ID, long CourseID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.CourseID = CourseID;
            ViewBag.TestID = ID;
            return View();
        }
        public ActionResult GetTest(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.GetTestByID(ID);
            if (data.UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddQuestionToTest(TestQuestionDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.GetQuestion(t.QuestionID).UserID != User.ID && ClassService.GetTestByID(t.TestID).UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(ClassService.AddQuestionToTest(t), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveQuestionFromTest(TestQuestionDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.GetQuestion(t.QuestionID).UserID != User.ID && ClassService.GetTestByID(t.TestID).UserID != User.ID)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(ClassService.RemoveQuestionFromTest(t), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateTopic(TopicDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = ClassService.CreateTopic(t.Name, User.ID);
            if (data != -1)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditTopic(TopicDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var check = db.Topics.Find(t.ID);
                if (check == null)
                {
                    return Json("Không tìm thấy chủ đề", JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
            }
            if (ClassService.EditTopic(t.Name, t.ID))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteTopic(TopicDTO t)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                var check = db.Topics.Find(t.ID);
                if (check == null)
                {
                    return Json("Không tìm thấy chủ đề", JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
            }
            if (ClassService.DeleteTopic(t.ID))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json("Fail", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Courses()
        {
            return View();
        }
        public ActionResult GetAllCourse()
        {
            var data = ClassService.GetAllClass();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckUserRole(CourseDetailDTO c)
        {
            return Json(ClassService.CheckUserRole(c.UserID, (long)c.CourseID), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListNotification(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.GetNotifications(ID), JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AddNotification(NotificationDTO n)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return Json(ClassService.AddNotification(n), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RemoveNotification(NotificationDTO n)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ClassService.RemoveNotification(n.ID) == 1)
            {
                string path = @"~/Content/Files/Notification/" + n.ID;

                if (System.IO.Directory.Exists(Server.MapPath(path)))
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Server.MapPath(path));
                    foreach (System.IO.FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    System.IO.Directory.Delete(Server.MapPath(path));

                }
                return Json(1, JsonRequestBehavior.AllowGet);

            }
            return Json(-1, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetListMonHoc()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lst = db.MonHocs.OrderBy(x=>x.MaMonHoc).ToList();
                List<MonHocDTO> data = new List<MonHocDTO>();
                foreach (var item in lst)
                {
                    MonHocDTO m = new MonHocDTO();
                    m.ID = item.MaMonHoc;
                    m.Name = item.TenMonHoc;
                    data.Add(m);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UploadNotiFiles()
        {
            string NotiID = Request["NotiID"];
            HttpFileCollectionBase files = Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                //System.IO.FileInfo fi = new System.IO.FileInfo(file.FileName);
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Content/Files/Notification/" + NotiID));
                string fname = System.IO.Path.Combine(Server.MapPath("~/Content/Files/Notification/" + NotiID), file.FileName);
                file.SaveAs(fname);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}