using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class CommentModel
    {
        public long ID { get; set; }
        public long? ParentCommentID { get; set; }
        public string NoiDung { get; set; }
        public string CreateDate { get; set; }
        public long? CreateBy { get; set; }
        public int ClassID { get; set; }
        public long CourseID { get; set; }
        public long LessionID { get; set; }
        public List<CommentModel> Reps { get; set; }
        public string HoTen { get; set; }



    }
}