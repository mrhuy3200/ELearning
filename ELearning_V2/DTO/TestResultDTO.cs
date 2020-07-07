using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class TestResultDTO
    {
        public long ID { get; set; }
        public long? TestID { get; set; }
        public long? UserID { get; set; }
        public long? CourseID { get; set; }
        public string Fullname { get; set; }
        public string TestName { get; set; }
        public int RightAnswer { get; set; }
        public double? TestResult { get; set; }
        public DateTime? TestDate { get; set; }
        public List<TestDetailDTO> TestDetails { get; set; }
    }
}