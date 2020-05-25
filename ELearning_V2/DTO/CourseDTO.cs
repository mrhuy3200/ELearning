using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class CourseDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Capacity { get; set; }
        public int? NumOfPeo { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<double> Price { get; set; }
        public string Schedule { get; set; }
        public string Condition { get; set; }
        public Nullable<int> Type { get; set; }
        public long Comments { get; set; }
        public string Username { get; set; }
        public int MaMonHoc { get; set; }
        public string TenMonHoc { get; set; }

    }
}