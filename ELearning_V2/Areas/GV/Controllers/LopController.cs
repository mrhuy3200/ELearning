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
    public class LopController : BaseController
    {
        // GET: GV/Lop
        public ActionResult MyClass()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetLop()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                List<LopModel> Lops = new List<LopModel>();
                List<Lop> lstLop = db.Lops.Where(x => x.MaGiangVien == session.ID).ToList();
                foreach (var item in lstLop)
                {
                    LopModel l = new LopModel();
                    l.MaLop = item.MaLop;
                    l.TenLop = item.TenLop;
                    l.TrangThai = item.TrangThai;
                    l.MaMonHoc = (int)item.MaMonHoc;
                    l.NgayBatDau = item.NgayBatDau.ToString();
                    l.NgayKetThuc = item.NgayKetThuc.ToString();
                    l.Image = item.Image;
                    l.SiSo = item.SiSo;
                    l.MoTa = item.MoTa;
                    Lops.Add(l);
                }
                return Json(Lops, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult ViewDetails(int id)
        {
            ViewBag.LopID = id;
            //ViewBag.FBhref = "http://localhost:49608/GV/Lop/ViewDetails/" + id.ToString();
            return View();
        }

        [HttpPost]
        public JsonResult Insert(LopModel lop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
                if (lop != null && db.Lops.Where(x => x.MaGiangVien == session.ID).Count() < 5)
                {
                    Lop l = new Lop();
                    l.TenLop = lop.TenLop;
                    l.TrangThai = true;
                    l.MaGiangVien = session.ID;
                    l.MaMonHoc = lop.MaMonHoc;
                    l.NgayBatDau = lop.StartDate;
                    l.Image = lop.Image;
                    l.SiSo = 0;
                    l.MoTa = lop.MoTa;
                    db.Lops.Add(l);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });

                }
            }
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

        [HttpGet]
        public JsonResult GetSV(int id)
        {
            ELearningDB db = new ELearningDB();
            List<HocVienModel> HVs = new List<HocVienModel>();
            var lstHV = db.HocViens.Where(s => s.Lops.Any(c => c.MaLop == id)).ToList();

            foreach (var item in lstHV)
            {
                HocVienModel hv = new HocVienModel();
                hv.ID = item.ID;
                hv.HoVaTen = item.HoVaTen;
                HVs.Add(hv);
            }

            return Json(HVs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveSV(AddNewHS_LOPModel data)
        {
            ELearningDB db = new ELearningDB();
            var sv = db.HocViens.Find(data.ID);
            var lop = db.Lops.Find(data.MaLop);
            if (sv != null && lop != null)
            {
                db.Entry(sv).Collection("Lop").Load();
                sv.Lops.Remove(lop);
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(LopModel UpdateLop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop lop = db.Lops.Find(UpdateLop.MaLop);
                if (lop == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    lop.MaLop = UpdateLop.MaLop;
                    lop.TenLop = UpdateLop.TenLop;
                    lop.MoTa = UpdateLop.MoTa;
                    lop.NgayBatDau = UpdateLop.StartDate;
                    lop.Image = UpdateLop.Image;
                    lop.MaMonHoc = UpdateLop.MaMonHoc;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop l = db.Lops.Find(id);
                if (l == null)
                {
                    return Json(new { success = false });
                }
                List<HocVien> lstHV = db.HocViens.Where(s => s.Lops.Any(c => c.MaLop == id)).ToList();
                foreach (var hv in lstHV)
                {
                    //db.Entry(hv).Collection("Lop").Load();
                    hv.Lops.Remove(l);
                }
                db.Lops.Remove(l);
                db.SaveChanges();
                return Json(new { success = true });
            }

        }

        [HttpPost]
        public JsonResult Publish(Lop updatedL)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop lop = db.Lops.Find(updatedL.MaLop);
                if (lop == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    lop.TrangThai = true;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        [HttpPost]
        public JsonResult Private(Lop updatedL)
        {
            using (ELearningDB db = new ELearningDB())
            {
                Lop lop = db.Lops.Find(updatedL.MaLop);
                if (lop == null)
                {
                    return Json(new { success = false });
                }
                else
                {
                    lop.TrangThai = false;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
        }

        //thêm học sinh vào lớp
        [HttpPost]
        public JsonResult AddNew(AddNewHS_LOPModel data)
        {
            using (ELearningDB db = new ELearningDB())
            {
                HocVien hv = db.HocViens.Find(data.ID);
                if (hv==null)
                {
                    return Json(new { success = false });

                }
                Lop l = db.Lops.Find(data.MaLop);
                l.HocViens.Add(hv);
                db.SaveChanges();
                return Json(new { success = true });
            }
        }

        [HttpGet]
        public JsonResult GetListMonHoc()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstMonHoc = db.MonHocs;
                List<MonHocModel> MonHocs = new List<MonHocModel>();
                foreach (var item in lstMonHoc)
                {
                    MonHocModel monhoc = new MonHocModel();
                    monhoc.MaMonHoc = item.MaMonHoc;
                    monhoc.TenMonHoc = item.TenMonHoc;
                    MonHocs.Add(monhoc);
                }
                return Json(MonHocs, JsonRequestBehavior.AllowGet);
            }
        }
    }
}