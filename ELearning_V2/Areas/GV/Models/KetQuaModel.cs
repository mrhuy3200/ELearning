using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class KetQuaModel
    {
        public long ID { get; set; }
        public double? Diem { get; set; }
        public int MaLopKiemTra { get; set; }
        public string NgayLam { get; set; }
        public string TenMonHoc { get; set; }
        public int? MaDeThi { get; set; }
        public string TenDeThi { get; set; }

    }
}