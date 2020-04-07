using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                HocVien hv = db.HocViens.Find(id);
                return View(hv);
            }
        }

        [HttpPost]
        public ActionResult Edit(HocVien hv)
        {
            using (ELearningDB db = new ELearningDB())
            {
                HocVien hocvien = db.HocViens.Find(hv.ID);
                hocvien.HoVaTen = hv.HoVaTen;
                hocvien.Email = hv.Email;
                hocvien.NgaySinh = hv.NgaySinh;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public JsonResult GetUser()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];

            using (ELearningDB db = new ELearningDB())
            {
                HocVienModel HV = new HocVienModel();
                HocVien hv = db.HocViens.Find(session.ID);
                HV.ID = hv.ID;
                HV.HoVaTen = hv.HoVaTen;
                HV.Email = hv.Email;
                if (hv.Image!=null)
                {
                    HV.Image = hv.Image;

                }
                else
                {
                    HV.Image = "Noimage";
                }
                HV.GioiTinh = (bool)hv.GioiTinh;
                HV.SDT = hv.SoDienThoai;
                HV.BirthDate = hv.NgaySinh.Value.ToString("dd/MM/yyyy");
                return Json(HV, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(HocVienModel updatedHV)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                HocVien existingSinhVien = db.HocViens.Find(session.ID);
                if (existingSinhVien == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    existingSinhVien.Email = updatedHV.Email;
                    existingSinhVien.SoDienThoai = updatedHV.SDT;
                    existingSinhVien.NgaySinh = updatedHV.NgaySinh;
                    if (updatedHV.Image!=null)
                    {
                        existingSinhVien.Image = updatedHV.Image;
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }


        [HttpPost]
        public JsonResult ChangePassword(PasswordModel pass)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session != null)
            {
                return Json(new { success = false });
            }
            using (ELearningDB db = new ELearningDB())
            {
                string p = Encryptor.MD5Hash(pass.OldPass);
                TaiKhoan tk = new TaiKhoan();
                tk = db.TaiKhoans.Find(session.ID);
                if (tk.Password != p)
                {
                    return Json(new { success = false });
                }
                tk.Password = Encryptor.MD5Hash(pass.NewPass);
                db.SaveChanges();
                return Json(new { success = true });
            }

        }


        [HttpGet]
        public ActionResult FirstLogin(int ID)
        {
            using (ELearningDB db = new ELearningDB())
            {

                HocVien hv = db.HocViens.Find(ID);
                if (hv.HoVaTen != null)
                {
                    Session.Abandon();
                    return RedirectToAction("Index", "Login");
                }
                HocVienModel hocvien = new HocVienModel();
                hocvien.ID = hv.ID;
                return View(hocvien);
            }
        }

        [HttpPost]
        public ActionResult FirstLogin(HocVienModel hv)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (ModelState.IsValid)
                {
                    HocVien hocvien = db.HocViens.Find(hv.ID);
                    TaiKhoan tk = db.TaiKhoans.Find(hv.ID);
                    hocvien.HoVaTen = hv.HoVaTen;
                    hocvien.Email = hv.Email;
                    hocvien.GioiTinh = hv.GioiTinh;
                    hocvien.SoDienThoai = hv.SDT;
                    hocvien.NgaySinh = hv.NgaySinh;
                    tk.Password = Encryptor.MD5Hash(hv.NewPass);
                    db.SaveChanges();
                    Session["Hoten"] = hv.HoVaTen;
                    return RedirectToAction("TrangChu", "Home");
                }
                return View(hv);
            }
        }

    }
}