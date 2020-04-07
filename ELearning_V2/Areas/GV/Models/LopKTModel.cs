using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class LopKTModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string TestDate { get; set; }
        public DateTime NgayKT { get; set; }
        public long GVID { get; set; }
        public int MonHocID { get; set; }
        public string TenMonHoc { get; set; }
        public int? MaDeThi { get; set; }
        public string TenDeThi { get; set; }
        public int ThoiGianThi { get; set; }
        public int SoCauHoi { get; set; }
        public string TenLopKT { get; set; }

    }
}