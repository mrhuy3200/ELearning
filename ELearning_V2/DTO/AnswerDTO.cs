using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class AnswerDTO
    {
        public long ID { get; set; }
        public string Content { get; set; }
        public Nullable<long> QuesionID { get; set; }
        public string Image { get; set; }

    }
}