using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class TestDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }
        public long? CourseID { get; set; }
        public int AmountQuestion { get; set; }
        public long UserID { get; set; }
        public List<QuestionDTO> Questions { get; set; }
        

    }
}