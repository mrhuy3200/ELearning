using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class ThongBaoModel
    {
        public long ID { get; set; }
        public string NoiDung { get; set; }
        public string NgayTao { get; set; }
        public long CreatedBy { get; set; }
        public int LopID { get; set; }
        public string TenGV { get; set; }

    }
}