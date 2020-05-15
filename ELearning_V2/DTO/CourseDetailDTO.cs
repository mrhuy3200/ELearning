using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class CourseDetailDTO
    {
        public long ID { get; set; }
        public Nullable<long> CourseID { get; set; }
        public Nullable<long> UserID { get; set; }
        public string Username { get; set; }

    }
}