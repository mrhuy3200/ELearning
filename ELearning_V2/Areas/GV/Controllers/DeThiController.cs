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
    public class DeThiController : BaseController
    {
        // GET: GV/DeThi
        public ActionResult Index()
        {
            ELearningDB db = new ELearningDB();

            List<MonHoc> mh = db.MonHocs.ToList();
            return View(mh);


        }

        public ActionResult DanhSachDeThi(int id) // truyền vào mã môn học
        {
            using (ELearningDB db = new ELearningDB())
            {
                var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                MonHoc mh = db.MonHocs.Find(id);
                ViewBag.MaMonHoc = id;
                ViewBag.TenMon = mh.TenMonHoc;
                return View();
            }
        }

        public ActionResult Details(int id) // truyền vào mã đề thi
        {
            using (ELearningDB db = new ELearningDB())
            {
                var dt = db.DeThis.Find(id);
                DeThiModel dethi = new DeThiModel();
                dethi.MaDeThi = dt.MaDeThi;
                dethi.MaMonHoc= (int)dt.MaMonHoc;
                dethi.SoCauHoi = (int)dt.SoCauHoi;
                return View(dethi);
            }

        }

        [HttpGet]
        public JsonResult GetAllDeThi(int id)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];

            using (ELearningDB db = new ELearningDB())
            {
                var lstDeThi = db.DeThis.Where(x => x.MaGiangVien == session.ID && x.MaMonHoc == id);
                List<DeThiModel> DeThis = new List<DeThiModel>();
                foreach (var item in lstDeThi)
                {
                    DeThiModel dethi = new DeThiModel();
                    dethi.MaDeThi = item.MaDeThi;
                    dethi.TenDeThi = item.TenDeThi;
                    dethi.MaMonHoc = (int)item.MaMonHoc;
                    dethi.MaGiangVien = (long)item.MaGiangVien;
                    dethi.TrangThai = (bool)item.TrangThai;
                    dethi.SoCauHoi = (int)item.SoCauHoi;
                    dethi.NgayTao = item.NgayTao.Value.ToString("dd/MM/yyyy");
                    dethi.NgayThi = item.NgayThi.Value.ToString("dd/MM/yyyy");
                    DeThis.Add(dethi);
                }
                return Json(DeThis, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Publish(DeThi dethi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                DeThi dt = db.DeThis.Find(dethi.MaDeThi);
                if (dt == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    dt.TrangThai = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult Private(DeThi dethi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                DeThi dt = db.DeThis.Find(dethi.MaDeThi);
                if (dt == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    dt.TrangThai = false;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult Insert(DeThi dethi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime utc = DateTime.UtcNow;
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

                var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                if (dethi == null)
                {
                    return Json(new { success = false });
                }
                dethi.MaGiangVien = session.ID;
                dethi.TrangThai = false;
                dethi.NgayTao = now;
                db.DeThis.Add(dethi);
                db.SaveChanges();
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public JsonResult Delete(DeThi dethi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                DeThi dt = db.DeThis.Find(dethi.MaDeThi);
                if (dt == null)
                {
                    return Json(new { success = false });
                }
                db.DeThis.Remove(dt);
                db.SaveChanges();
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public JsonResult Update(DeThi dethi)
        {
            using (ELearningDB db = new ELearningDB())
            {
                DeThi dt = db.DeThis.Find(dethi.MaDeThi);
                if (dt == null)
                {
                    return Json(new { success = false });
                }
                dt.TenDeThi = dethi.TenDeThi;
                dt.SoCauHoi = dethi.SoCauHoi;
                dt.NgayThi = dethi.NgayThi;
                db.SaveChanges();
                return Json(new { success = true });
            }


        }

    }
}