using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class CommentDTO
    {
        public long ID { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> CreateBy { get; set; }
        public string Fullname { get; set; }
        public string UserImage { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<long> CourseID { get; set; }
        public Nullable<long> LessionID { get; set; }
        public List<CommentDTO> Replies { get; set; }
        public int Rate { get; set; }
    }
}