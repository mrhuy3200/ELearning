using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class DeThiModel
    {
        public long MaDeThi { get; set; }
        public string TenDeThi { get; set; }
        public int MaMonHoc { get; set; }
        public long MaGiangVien { get; set; }
        public string TenGiangVien { get; set; }
        public bool TrangThai { get; set; }
        public string NgayTao { get; set; }
        public int SoCauHoi { get; set; }
        public string NgayThi { get; set; }


    }
}