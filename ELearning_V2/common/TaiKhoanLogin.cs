using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.common
{
    [Serializable]
    public class TaiKhoanLogin
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public int loai { get; set; }
    }
}