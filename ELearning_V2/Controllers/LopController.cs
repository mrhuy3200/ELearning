using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.DTO;
using ELearning_V2.Models;
using ELearning_V2.Service;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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
                data = ClassService.GetClassByUserID(User.ID);
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

        public ActionResult GetLessionByClassID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                List<LessionDTO> data = new List<LessionDTO>();
                data = ClassService.GetLessionByClassID(ID);
                if (db.Courses.Find(ID).UserID != User.ID)
                {
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
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
            using (ELearningDB db = new ELearningDB())
            {
                Course co = new Course();
                co.Name = c.Name;
                co.Capacity = c.Capacity;
                co.Description = c.Description;
                co.Price = c.Price;
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
                if (db.Lessions.Find(l.ID).Course.UserID != User.ID)
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

        public ActionResult CreateLession(long ID)
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
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
            }
            LessionDTO l = new LessionDTO();
            l.CourseID = ID;
            return View(l);
        }

        public ActionResult EditLession(long ID)
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
                if (check.Course.UserID != User.ID)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                LessionDTO l = new LessionDTO();
                l = ClassService.GetLessionByID(ID);
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
                var check = db.Courses.Find(l.CourseID);
                if (check == null)
                {
                    return Json("Không tìm thấy lớp học", JsonRequestBehavior.AllowGet);
                }
                if (check.UserID != User.ID)
                {
                    return Json("Không đủ quyền hạn", JsonRequestBehavior.AllowGet);
                }
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
                if (checkLession.Course.UserID != User.ID)
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
                if (db.Lessions.Find(l.ID).Course.UserID != User.ID)
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

        public ActionResult LessionDetail(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                Lession l = new Lession();
                l = db.Lessions.Find(ID);
                if (l == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                if (l.Course.UserID != User.ID)
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }
                return View(l);
            }

        }

        public ActionResult GetLessionByID(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                LessionDTO l = new LessionDTO();
                l = ClassService.GetLessionByID(ID);
                return Json(l, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetCommentByID(CommentDTO c)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Login");
            }
            using (ELearningDB db = new ELearningDB())
            {
                if (c.LessionID != null)
                {
                    var check = db.Lessions.Find(c.LessionID);
                    if (check.Course.UserID == User.ID)
                    {
                        var data = ClassService.GetCommentByID((long)c.LessionID, 3);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                if (c.CourseID != null)
                {
                    var check = db.Courses.Find(c.CourseID);
                    if (check.UserID == User.ID)
                    {
                        var data = ClassService.GetCommentByID((long)c.CourseID, 2);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                if (c.ClassID != null)
                {
                    var check = db.Lops.Find(c.ClassID);
                    if (check.MaGiangVien == User.ID)
                    {
                        var data = ClassService.GetCommentByID((long)c.CourseID, 1);
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                    return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);
                }
                return Json("Không có dữ liệu", JsonRequestBehavior.AllowGet);

            }
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
    }
}