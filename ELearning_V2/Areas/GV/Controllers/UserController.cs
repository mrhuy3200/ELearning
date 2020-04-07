using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Controllers
{
    public class UserController : BaseController
    {
        // GET: GV/User
        public ActionResult Index()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session == null || session.loai != 2)
            {
                return RedirectToAction("Index", "Login", "GV");
            }
            return View("Index");
        }

        //GET: GV/User/GetUser
        [HttpGet]
        public JsonResult GetUser()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];

            using (ELearningDB db = new ELearningDB())
            {
                GVModel GV = new GVModel();
                GiangVien gv = db.GiangViens.Find(session.ID);
                GV.ID = gv.ID;
                GV.HoVaTen = gv.HoVaTen;
                GV.Email = gv.Email;
                GV.Image = gv.Image;
                GV.GioiTinh = (bool)gv.GioiTinh;
                GV.TenMonHoc = gv.MonHoc.TenMonHoc;
                GV.MaMonHoc = (int)gv.MaMonHoc;
                GV.SDT = gv.SoDienThoai;
                return Json(GV, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(GVModel updatedGV)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                GiangVien existingGV = db.GiangViens.Find(session.ID);
                if (existingGV == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    existingGV.Email = updatedGV.Email;
                    existingGV.SoDienThoai = updatedGV.SDT;
                    existingGV.MaMonHoc = updatedGV.MaMonHoc;
                    if (updatedGV.Image != null)
                    {
                        existingGV.Image = updatedGV.Image;
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
                GiangVien gv = db.GiangViens.Find(ID);
                if (gv.HoVaTen != null)
                {
                    Session.Abandon();
                    return RedirectToAction("Index", "Login", new { area=""});
                }
                HocVienModel hocvien = new HocVienModel();

                GiangVienModel giangvien = new GiangVienModel();
                giangvien.ID = gv.ID;
                var lstMonHoc = db.MonHocs;
                List<MonHocModel> MonHocs = new List<MonHocModel>();
                foreach (var item in lstMonHoc)
                {
                    MonHocModel mh = new MonHocModel();
                    mh.MaMonHoc = item.MaMonHoc;
                    mh.TenMonHoc = item.TenMonHoc;
                    MonHocs.Add(mh);
                }
                SelectList lstMon = new SelectList(MonHocs, "MaMonHoc", "TenMonHoc");
                ViewBag.ListMonHoc = lstMon;
                return View(giangvien);
            }
        }

        [HttpPost]
        public ActionResult FirstLogin(GiangVienModel gv)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (ModelState.IsValid)
                {
                    GiangVien giangvien = db.GiangViens.Find(gv.ID);
                    TaiKhoan tk = db.TaiKhoans.Find(gv.ID);
                    giangvien.HoVaTen = gv.HoVaTen;
                    giangvien.Email = gv.Email;
                    giangvien.GioiTinh = gv.GioiTinh;
                    giangvien.MaMonHoc = gv.MaMonHoc;
                    giangvien.SoDienThoai = gv.SDT;
                    tk.Password = Encryptor.MD5Hash(gv.NewPass);
                    db.SaveChanges();
                    return RedirectToAction("TrangChu", "HomeGV", new { area = "GV" });
                }
                var lstMonHoc = db.MonHocs;
                List<MonHocModel> MonHocs = new List<MonHocModel>();
                foreach (var item in lstMonHoc)
                {
                    MonHocModel mh = new MonHocModel();
                    mh.MaMonHoc = item.MaMonHoc;
                    mh.TenMonHoc = item.TenMonHoc;
                    MonHocs.Add(mh);
                }
                SelectList lstMon = new SelectList(MonHocs, "MaMonHoc", "TenMonHoc");
                ViewBag.ListMonHoc = lstMon;
                return View(gv);
            }
        }

    }
}