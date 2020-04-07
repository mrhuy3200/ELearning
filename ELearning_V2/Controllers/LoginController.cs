using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            if (TempData["ActiveResult"] != null)
            {
                ViewBag.ActiveResult = TempData["ActiveResult"].ToString();
            }

            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                string pass = Encryptor.MD5Hash(model.PassWord);
                using (ELearningDB db = new ELearningDB())
                {
                    var res = db.TaiKhoans.Where(x => x.Username == model.UserName && x.Password == pass).FirstOrDefault();
                    if (res != null)
                    {
                        if (res.TrangThai != false)
                        {
                            var userSession = new TaiKhoanLogin();
                            userSession.ID = res.ID;
                            userSession.loai = res.Role;
                            userSession.UserName = res.Username;
                            Session.Add(CommonConstants.USER_SESSION, userSession);

                            if (res.Role == 2)
                            {
                                GiangVien gv = db.GiangViens.Find(res.ID);
                                if (gv.HoVaTen == null)
                                {
                                    return RedirectToAction("FirstLogin", "User", new { area = "GV", id = gv.ID });

                                }
                                Session["Hoten"] = gv.HoVaTen;
                                return RedirectToAction("TrangChu", "HomeGV", new { area = "GV" });
                            }
                            else
                            {
                                if (res.Role == 3)
                                {
                                    HocVien hv = db.HocViens.Find(res.ID);
                                    if (hv.HoVaTen == null)
                                    {
                                        return RedirectToAction("FirstLogin", "User", new { id = hv.ID });

                                    }

                                    Session["Hoten"] = hv.HoVaTen;
                                    return RedirectToAction("TrangChu", "Home");
                                }
                                else
                                {
                                    if (res.Role == 1)
                                    {
                                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                                    }
                                    else
                                    {
                                        if (res.Role == 4)
                                        {
                                            NguoiDung user = db.NguoiDungs.Find(res.ID);
                                            Session["Hoten"] = user.HoVaTen;
                                            return RedirectToAction("TrangChu", "Home");
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("", "Tài khoản này hiện đang bị khóa");
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

        public JsonResult APILogin(LoginModel model)
        {
            string pass = Encryptor.MD5Hash(model.PassWord);
            using (ELearningDB db = new ELearningDB())
            {
                var res = db.TaiKhoans.Where(x => x.Username == model.UserName && x.Password == pass).FirstOrDefault();
                if (res!=null)
                {
                    return Json(res.ID);
                }
                return Json(false);

            }

        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                using (ELearningDB db = new ELearningDB())
                {

                    TaiKhoan tk = new TaiKhoan();
                    tk.Username = model.UserName;
                    tk.Password = Encryptor.MD5Hash(model.Password);
                    tk.Role = 4;
                    tk.TrangThai = false;
                    var res = db.TaiKhoans.Where(x => x.Username == model.UserName).Count();
                    var res1 = db.NguoiDungs.Where(x => x.Email == model.Email).Count();
                    
                    if (res != 0)
                    {
                        ViewBag.UserNameError = "Tên người dùng đã tồn tại";
                        return View(model);
                    }
                    if (res1 != 0)
                    {
                        ViewBag.EmailError = "Email đã sử dụng";
                        return View(model);
                    }

                    if (res == 0 && res1 == 0)
                    {
                        db.TaiKhoans.Add(tk);
                        db.SaveChanges();
                    }
                    TaiKhoan ID = db.TaiKhoans.Where(x => x.Username == model.UserName).FirstOrDefault();
                    NguoiDung u = db.NguoiDungs.Find(ID.ID);
                    u.HoVaTen = model.FullName;
                    u.Email = model.Email;
                    u.Image = null;
                    u.SoDu = 20000;
                    u.MaXacNhan = GenerateCode();
                    db.SaveChanges();
                    SendEmail(u);
                    TempData["RegisterResult"] = "Đăng ký thành công"; 
                    return RedirectToAction("Home","Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ActiveAccount(long ID, string AuthenticationCode)
        {
            using (ELearningDB db = new ELearningDB())
            {
                NguoiDung res = db.NguoiDungs.Where(x => x.ID == ID).FirstOrDefault();
                if (res != null)
                {
                    //if (res.MaXacNhan == null)
                    //{
                    //    TempData["ActiveResult"] = "Tài khoản đã được kích hoạt trước đó";
                    //    return RedirectToAction("TrangChu", "Home");
                    //}
                    TaiKhoan tk = db.TaiKhoans.Find(ID);
                    if (tk.TrangThai)
                    {
                        TempData["ActiveResult"] = "Tài khoản đã được kích hoạt trước đó";
                        return RedirectToAction("TrangChu", "Home");
                    }
                    if (res.MaXacNhan == AuthenticationCode)
                    {
                        tk.TrangThai = true;
                        res.MaXacNhan = null;
                        db.SaveChanges();
                        TempData["ActiveResult"] = "Tài khoản vừa được kích hoạt";
                        return RedirectToAction("Index", "Login");
                    }
                }
                TempData["ActiveResult"] = "Tài khoản chưa được kích hoạt";
                return RedirectToAction("TrangChu", "Home");
            }
        }

        private string GenerateCode()
        {
            int length = 32;
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            char letter;
            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        private void SendEmail(NguoiDung user)
        {
                var senderEmail = new MailAddress("cloneelsword2@gmail.com", "ELearning");
                var receiverEmail = new MailAddress(user.Email, user.HoVaTen);
                var password = "huynhthanhhuy";
                var sub = "Kích hoạt tài khoản";
                var body = string.Format("Xin chào {0} <BR/>Cảm ơn vì đã đăng ký thành viên, vui lòng nhấn vào đường dẫn sau để kích hoạt tài khoản của bạn: http://localhost:49608/Login/ActiveAccount/{1}/{2} ", user.HoVaTen, user.ID, user.MaXacNhan);
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = sub,
                    Body = body
                })
                {
                    smtp.Send(mess);
                }
            
        }
    }
}