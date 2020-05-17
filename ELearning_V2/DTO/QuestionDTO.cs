using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.DTO
{
    public class QuestionDTO
    {
        public long ID { get; set; }
        public string Content { get; set; }
        public long? AnswerID { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public long? UserID { get; set; }
        public int? Level { get; set; }
        public string Solution { get; set; }
        public List<AnswerDTO> Answers { get; set; }
        public List<TopicDTO> Topics { get; set; }

    }
}