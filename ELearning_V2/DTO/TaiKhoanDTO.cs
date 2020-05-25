using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class TaiKhoanDTO
    {
        public long ID { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public bool TrangThai { get; set; }
        public double Balance { get; set; }
        public string Info { get; set; }
        public string ConnectionID { get; set; }


        public long CourseID { get; set; }
    }
}