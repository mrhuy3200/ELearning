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
    public class AppAPIController : Controller
    {

        public JsonResult GetLopKT(int id)
        {
            using (ELearningDB db = new ELearningDB())
            {
                HocVien hv = db.HocViens.Find(id);
                TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime utc = DateTime.UtcNow;
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date); 
                var DaKiemTra = new List<LopKiemTra>();
                //var lstLopKT = db.LopKiemTra.Where(x => x.HocVien.Any(r => r.ID == id)).ToList();

                //var lstBaiLam = db.BaiLam.Where(x => x.MaHocVien == id).ToList();
                var lstbl = (from x in db.BaiLams
                             where x.MaHocVien == id
                             select x.MaLopKiemTra).ToList();
                foreach (var item in lstbl)
                {
                    var l = db.LopKiemTras.Where(x => x.HocViens.Any(r => r.ID == id) && x.ID == item).FirstOrDefault();
                    DaKiemTra.Add(l);
                }
                var lstLopKT = db.LopKiemTras.Where(x => x.HocViens.Any(r => r.ID == id)).ToList().Except(DaKiemTra);
                List<LopKTModel> Lops = new List<LopKTModel>();
                foreach (var item in lstLopKT)
                {
                    if (item.TestDate.Value.Date == now.Date)
                    {
                        LopKTModel l = new LopKTModel();
                        l.ID = item.ID;
                        l.Name = item.Name;
                        l.MaDeThi = item.MaDeThi;
                        l.SoCauHoi = (int)item.DeThi.SoCauHoi;
                        l.ThoiGianThi = (int)item.ThoiGianThi;
                        Lops.Add(l);
                    }
                }
                return Json(Lops, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetCauHoiKT(LopKTModel lop)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstCauHoi = db.CauHois.Where(x => x.DeThis.Any(r => r.MaDeThi == lop.MaDeThi)).ToList();
                List<CauHoiModel> CauHois = new List<CauHoiModel>();
                foreach (var item in lstCauHoi)
                {
                    CauHoiModel ch = new CauHoiModel();
                    ch.NoiDung = item.NoiDung;
                    ch.BieuThuc = item.BieuThuc;
                    ch.CauA = item.CauA;
                    ch.BieuThucA = item.BieuThucA;
                    ch.CauB = item.CauB;
                    ch.BieuThucB = item.BieuThucB;
                    ch.CauC = item.CauC;
                    ch.BieuThucC = item.BieuThucC;
                    ch.CauD = item.CauD;
                    ch.BieuThucD = item.BieuThucD;
                    ch.DapAn = (int)item.DapAn;
                    CauHois.Add(ch);
                }
                return Json(CauHois, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult KetQua(KetQuaModel kq)
        {
            TimeZoneInfo date = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime utc = DateTime.UtcNow;
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(utc, date);

            try
            {
                BaiLam bailam = new BaiLam();
                bailam.MaLopKiemTra = kq.MaLopKiemTra;
                bailam.MaHocVien = kq.ID;
                bailam.TongDiem = kq.Diem;
                bailam.NgayKiemTra = now;
                using (ELearningDB db = new ELearningDB())
                {
                    var bl = db.BaiLams.Where(x => x.MaHocVien == kq.ID).ToList();
                    foreach (var item in bl)
                    {
                        if (item.MaHocVien == kq.ID && item.MaLopKiemTra == kq.MaLopKiemTra)
                        {
                            return Json(false);
                        }
                    }
                    db.BaiLams.Add(bailam);
                    db.SaveChanges();
                    return Json(true);
                }

            }
            catch (Exception)
            {

                return Json(false);

            }
        }

    }
}