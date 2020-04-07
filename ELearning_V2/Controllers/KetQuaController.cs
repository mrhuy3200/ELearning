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
    public class KetQuaController : Controller
    {
        // GET: KetQua

        public ActionResult Index()
        {
            var session = (TaiKhoanLogin)Session[CommonConstants.USER_SESSION];
            if (session != null && session.loai == 3)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    List<KetQuaModel> KetQuas = new List<KetQuaModel>();
                    var lstKQ = db.BaiLams.Where(x => x.MaHocVien == session.ID).ToList();
                    foreach (var item in lstKQ)
                    {
                        KetQuaModel kq = new KetQuaModel();
                        kq.TenMonHoc = item.LopKiemTra.MonHoc.TenMonHoc;
                        kq.MaDeThi = item.LopKiemTra.MaDeThi;
                        kq.TenDeThi = item.LopKiemTra.Name;
                        kq.NgayLam = item.NgayKiemTra.Value.ToString("dd/MM/yyyy");
                        kq.Diem = item.TongDiem;
                        KetQuas.Add(kq);
                    }
                    return View(KetQuas);
                }
            }
            else
            {
                TempData["Error"] = "Bạn chưa đăng nhập";
                return RedirectToAction("TrangChu", "Home");
            }

        }
    }
}