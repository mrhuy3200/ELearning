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
                if (res!=0)
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
                return Json(id, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CourseDetail(long ID)
        {
            var User = (TaiKhoan)Session["User"];
            if (User == null)
            {
                return RedirectToAction("Login", "Index");
            }
            using (ELearningDB db = new ELearningDB())
            {
                Course c = new Course();
                c = db.Courses.Find(ID);
                if (c.UserID != User.ID)
                {
                    return Json("Không tìm thấy lớp học", JsonRequestBehavior.AllowGet);
                }
                return View(c);
            }
        }
    }
}