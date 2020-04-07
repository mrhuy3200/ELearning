using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.common
{
    public class GVModel
    {
        public long ID { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public bool GioiTinh { get; set; }
        public int MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }
        public string SDT { get; set; }

    }
}