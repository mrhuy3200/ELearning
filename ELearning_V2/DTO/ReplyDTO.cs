using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class ReplyDTO
    {
        public string NoiDung { get; set; }
        public long CommentID { get; set; }
        public long CreateBy { get; set; }
    }
}