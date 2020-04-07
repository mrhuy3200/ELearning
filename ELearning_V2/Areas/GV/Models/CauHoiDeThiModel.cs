using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class CauHoiDeThiModel
    {
        public long DeThiID { get; set; }
        public long CauHoiID { get; set; }
        public int ChuongID { get; set; }
        public int DoKho { get; set; }
        public int SoCauHoi { get; set; }
        public int MaMonHoc { get; set; }

    }
}