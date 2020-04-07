using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.common.API_Model
{
    public class TaiKhoanModel
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string key { get; set; }
        public bool TrangThai { get; set; }


    }
}