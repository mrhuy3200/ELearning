using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class LessionDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string URL { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> CourseID { get; set; }
        public long View { get; set; }
        public string Username { get; set; }
        public string UserAvatar { get; set; }
        public string UserInfo { get; set; }
        public DateTime CreateDate { get; set; }
        public long Comment { get; set; }
        public string Image { get; set; }

    }
}