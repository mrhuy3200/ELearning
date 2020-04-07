using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.Admin.Models
{
    public class NguoiDungModel
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }
        public string HoVaTen { get; set; }
        public string Email { get; set; }
        public bool TrangThai { get; set; }

    }
}