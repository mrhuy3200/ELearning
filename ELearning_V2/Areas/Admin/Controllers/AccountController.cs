using ELearning_V2.Areas.Admin.Models;
using ELearning_V2.common;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(ImportExcel importExcel)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Assets/Upload/" + importExcel.file.FileName);
                importExcel.file.SaveAs(path);

                string excelConnectionString = @"Provider='Microsoft.ACE.OLEDB.12.0';Data Source='" + path + "';Extended Properties='Excel 12.0 Xml;IMEX=1'";
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);

                //Sheet Name
                excelConnection.Open();
                string tableName = excelConnection.GetSchema("Tables").Rows[0]["TABLE_NAME"].ToString();
                excelConnection.Close();
                //End

                OleDbCommand cmd = new OleDbCommand("Select Username, Password, Role from [" + tableName + "]", excelConnection);

                excelConnection.Open();

                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();
                using (ELearningDB db = new ELearningDB())
                {
                    SqlConnection sqlCon = new SqlConnection(db.Database.Connection.ConnectionString);
                    sqlCon.Open();
                    using (SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlCon))
                    {
                        sqlBulk.DestinationTableName = "TaiKhoan";
                        //Mappings
                        sqlBulk.ColumnMappings.Add("Username", "Username");
                        sqlBulk.ColumnMappings.Add("Password", "Password");
                        sqlBulk.ColumnMappings.Add("Role", "Role");

                        sqlBulk.WriteToServer(dReader);
                        excelConnection.Close();

                    }
                    sqlCon.Close();
                    var lstGVInserted = db.TaiKhoans.Where(x => x.Role == 2 && x.GiangVien == null);
                    List<GiangVien> lstGV = new List<GiangVien>();
                    foreach (var item in lstGVInserted)
                    {
                        GiangVien gv = new GiangVien();
                        gv.ID = item.ID;
                        db.GiangViens.Add(gv);
                    }
                    var lstHVInserted = db.TaiKhoans.Where(x => x.Role == 3 && x.HocVien == null);
                    List<HocVien> lstHV = new List<HocVien>();
                    foreach (var item in lstHVInserted)
                    {
                        HocVien hv = new HocVien();
                        hv.ID = item.ID;
                        db.HocViens.Add(hv);
                    }
                    db.SaveChanges();

                }

                //Give your Destination table name

                ViewBag.Result = "Successfully Imported";
            }
            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public JsonResult GetAllAccount()
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstAcc = db.TaiKhoans.OrderBy(x => x.Role).ThenBy(a => a.ID);
                List<NguoiDungModel> Users = new List<NguoiDungModel>();
                foreach (var item in lstAcc)
                {
                    NguoiDungModel ng = new NguoiDungModel();
                    ng.ID = item.ID;
                    ng.Username = item.Username;
                    ng.TrangThai = (bool)item.TrangThai;
                    ng.Role = item.Role;
                    if (ng.Role == 2)
                    {
                        ng.HoVaTen = item.GiangVien.HoVaTen;
                        ng.Email = item.GiangVien.Email;
                    }
                    else
                    {
                        if (ng.Role == 3)
                        {
                            ng.HoVaTen = item.HocVien.HoVaTen;
                            ng.Email = item.HocVien.Email;

                        }
                        else
                        {
                            if (ng.Role == 4)
                            {
                                ng.HoVaTen = item.NguoiDung.HoVaTen;
                                ng.Email = item.NguoiDung.Email;

                            }
                        }
                    }
                    Users.Add(ng);
                }
                return Json(Users, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Insert(NguoiDungModel user)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk.Username = user.Username;
                    tk.Password = "25f9e794323b453885f5181f1b624d0b";
                    tk.Role = user.Role;
                    tk.TrangThai = true;
                    db.TaiKhoans.Add(tk);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult Lock(NguoiDungModel user)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk = db.TaiKhoans.Find(user.ID);
                    tk.TrangThai = false;
                    db.SaveChanges();
                    return Json(new { success = true });

                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult UnLock(NguoiDungModel user)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    TaiKhoan tk = new TaiKhoan();
                    tk = db.TaiKhoans.Find(user.ID);
                    tk.TrangThai = true;
                    db.SaveChanges();
                    return Json(new { success = true });

                }
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

    }
}