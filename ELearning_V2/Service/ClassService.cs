using ELearning_V2.DTO;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ELearning_V2.Service
{
    public class ClassService
    {
        public static List<CourseDTO> GetClassByUserID(long UserID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lst = db.Courses.Where(x => x.UserID == UserID).ToList();
                List<CourseDTO> data = new List<CourseDTO>();
                foreach (var item in lst)
                {
                    CourseDTO l = new CourseDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.Capacity = item.Capacity;
                    l.NumOfPeo = db.CourseDetails.Where(x => x.CourseID == item.ID).Count();
                    l.Description = item.Description;
                    l.Image = item.Image;
                    l.Status = item.Status;
                    l.Price = item.Price;
                    l.Schedule = item.Schedule;
                    l.Condition = item.Condition;
                    l.Type = item.Type;
                    data.Add(l);
                }
                return data;
            }
        }
    }
}