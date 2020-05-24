using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class NotificationDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Nullable<long> CourseID { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}