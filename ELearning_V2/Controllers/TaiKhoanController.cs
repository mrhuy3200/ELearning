using ELearning_V2.Areas.GV.Models;
using ELearning_V2.common;
using ELearning_V2.common.API_Model;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ELearning_V2.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(ImportExcel importExcel)
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

                OleDbCommand cmd = new OleDbCommand("Select * from [" + tableName + "]", excelConnection);

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

            return View();
        }

        [HttpPost]
        public JsonResult Insert(TaiKhoanModel tk)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var Key = db.KeyAPIs.Where(x => x.Name == "KeyAccount").FirstOrDefault();
                if (tk.key == Key.Value)
                {
                    TaiKhoan taikhoan = new TaiKhoan();
                    taikhoan.Username = tk.Username;
                    taikhoan.Role = tk.Role;
                    taikhoan.Password = "25f9e794323b453885f5181f1b624d0b";
                    taikhoan.TrangThai = true;
                    db.TaiKhoans.Add(taikhoan);
                    db.SaveChanges();
                    return Json(true);

                }
                else
                {
                    return Json(false);
                }
            }
        }

        [HttpPost]
        public JsonResult GetTK(TaiKhoanModel tk)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var Key = db.KeyAPIs.Where(x => x.Name == "KeyAccount").FirstOrDefault();
                if (tk.key == Key.Value)
                {
                    List<TaiKhoan> lstTaiKhoan = db.TaiKhoans.Where(x => x.Role != 1).ToList();
                    List<TaiKhoanModel> TaiKhoans = new List<TaiKhoanModel>();
                    foreach (var item in lstTaiKhoan)
                    {
                        TaiKhoanModel taikhoan = new TaiKhoanModel();
                        taikhoan.ID = item.ID;
                        taikhoan.Username = item.Username;
                        taikhoan.Role = item.Role;
                        taikhoan.Username = item.Username;
                        taikhoan.TrangThai = (bool)item.TrangThai;
                        TaiKhoans.Add(taikhoan);
                    }
                    return Json(TaiKhoans,JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(false);
                }
            }

        }
    }
}