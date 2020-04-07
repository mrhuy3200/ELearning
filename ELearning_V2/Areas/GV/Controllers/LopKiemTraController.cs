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
    public class LopKiemTraController : BaseController
    {
        // GET: GV/LopKiemTra
        public ActionResult QuanLyKiemTra()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];

            using (ELearningDB db = new ELearningDB())
            {

            }
            return View();
        }

        [HttpGet]
        public JsonResult GetLopKT()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                var lstLopKT = db.LopKiemTras.Where(x => x.GVID == session.ID);
                List<LopKTModel> LopKTs = new List<LopKTModel>();
                foreach (var item in lstLopKT)
                {
                    LopKTModel lopKT = new LopKTModel();
                    lopKT.ID = item.ID;
                    lopKT.Name = item.Name;
                    lopKT.TestDate = item.TestDate.Value.ToString("dd/MM/yyyy");
                    lopKT.GVID = item.GVID;
                    lopKT.MonHocID = item.MaMonHoc;
                    lopKT.ThoiGianThi = (int)item.ThoiGianThi;
                    //lopKT.MaDeThi = item.MaDeThi;
                    if (item.MaDeThi == null)
                    {
                        lopKT.TenDeThi = "Chưa chọn đề thi";
                    }
                    else
                    {
                        lopKT.TenDeThi = item.DeThi.TenDeThi;

                    }
                    LopKTs.Add(lopKT);
                }
                return Json(LopKTs, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetDeThi(int id)
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
                    DeThis.Add(dethi);
                }
                return Json(DeThis, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetHocVien(int id)
        {
            ELearningDB db = new ELearningDB();
            List<HocVienModel> HVs = new List<HocVienModel>();
            var lstHV = db.HocViens.Where(s => s.LopKiemTras.Any(c => c.ID == id)).ToList();

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
        public JsonResult Create(LopKTModel lop)
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            using (ELearningDB db = new ELearningDB())
            {
                if (lop != null)
                {
                    LopKiemTra lopkt = new LopKiemTra();
                    lopkt.Name = lop.Name;
                    lopkt.TestDate = lop.NgayKT.AddDays(1);
                    lopkt.MaMonHoc = lop.MonHocID;
                    lopkt.MaDeThi = null;
                    lopkt.GVID = session.ID;
                    lopkt.ThoiGianThi = lop.ThoiGianThi;
                    db.LopKiemTras.Add(lopkt);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }

        }

        [HttpPost]
        public JsonResult Delete(LopKTModel lop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (lop != null)
                {
                    LopKiemTra lopkt = db.LopKiemTras.Find(lop.ID);
                    db.LopKiemTras.Remove(lopkt);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult Update(LopKTModel lop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (lop != null)
                {
                    var l = db.LopKiemTras.Find(lop.ID);
                    if (l == null)
                    {
                        return Json(new { success = false });
                    }
                    l.Name = lop.Name;
                    l.MaMonHoc = lop.MonHocID;
                    l.TestDate = lop.NgayKT.AddDays(1);
                    l.ThoiGianThi = lop.ThoiGianThi;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });

            }
        }

        [HttpPost]
        public JsonResult UpdateDeThi(LopKTModel lop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                if (lop != null)
                {
                    LopKiemTra lopkt = db.LopKiemTras.Find(lop.ID);
                    DeThi dt = db.DeThis.Find(lop.MaDeThi);
                    string tendt = dt.TenDeThi;
                    lopkt.MaDeThi = lop.MaDeThi;
                    db.SaveChanges();
                    
                    return Json(new { success = tendt });
                }
                return Json(new { success = false });

            }
        }

        [HttpPost]
        public JsonResult AddNew(AddNewHS_LOPModel data)
        {
            using (ELearningDB db = new ELearningDB())
            {
                HocVien hv = db.HocViens.Find(data.ID);
                LopKiemTra l = db.LopKiemTras.Find(data.MaLop);
                l.HocViens.Add(hv);
                db.SaveChanges();
                return Json(new { success = true });
            }
        }

        [HttpPost]
        public JsonResult RemoveSV(AddNewHS_LOPModel data)
        {
            ELearningDB db = new ELearningDB();
            var sv = db.HocViens.Find(data.ID);
            var lop = db.LopKiemTras.Find(data.MaLop);
            if (sv != null && lop != null)
            {
                db.Entry(sv).Collection("Lop").Load();
                sv.LopKiemTras.Remove(lop);
                db.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

    }
}