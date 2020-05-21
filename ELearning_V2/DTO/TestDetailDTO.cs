using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class TestDetailDTO
    {
        public long ID { get; set; }
        public long QuestionID { get; set; }
        public long Answer { get; set; }
        public long TestID { get; set; }
        public long UserID { get; set; }
    }
}