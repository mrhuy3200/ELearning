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
    public class LoginController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var res = db.TaiKhoan.Count(x => x.Username == model.UserName);
                    if (res > 0)
                    {
                        var user = db.TaiKhoan.SingleOrDefault(x => x.Username == model.UserName);
                        var userSession = new TaiKhoanLogin();
                        userSession.ID = user.ID;
                        userSession.loai = user.Role;
                        userSession.UserName = user.Username;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        if (user.Role == 2)
                        {
                            GiangVien gv = db.GiangVien.Find(user.ID);
                            Session["Hoten"] = gv.HoVaTen;
                            return RedirectToAction("TrangChu", "HomeGV", new { area = "GV" });
                        }
                        else
                        {
                            if (user.Role == 3)
                            {
                                HocVien hv = db.HocVien.Find(user.ID);
                                Session["Hoten"] = hv.HoVaTen;
                                return RedirectToAction("TrangChu", "Home");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng");
                    }
                }

            }
            return View("Index");
        }
        public ActionResult Logout()
        {
            //Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}