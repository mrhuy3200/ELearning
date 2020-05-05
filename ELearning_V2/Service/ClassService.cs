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

        public static CourseDTO GetClassByID(long ID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var c = db.Courses.Find(ID);
                CourseDTO cou = new CourseDTO();
                cou.ID = c.ID;
                cou.Name = c.Name;
                cou.Capacity = c.Capacity;
                cou.NumOfPeo = db.CourseDetails.Where(x => x.CourseID == c.ID).Count();
                cou.Description = c.Description;
                cou.Image = c.Image;
                cou.Status = c.Status;
                cou.Price = c.Price;
                cou.Schedule = c.Schedule;
                cou.Condition = c.Condition;
                cou.Type = c.Type;
                cou.UserID = c.UserID;
                return cou;
            }
        }

        public static List<TaiKhoanDTO> GetMemberByClassID(long ID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstMember = db.CourseDetails.Where(x => x.CourseID == ID).Select(a => a.UserID);
                var lst = db.TaiKhoans.Where(x => lstMember.Contains(x.ID)).ToList();
                List<TaiKhoanDTO> data = new List<TaiKhoanDTO>();
                foreach (var item in lst)
                {
                    TaiKhoanDTO l = new TaiKhoanDTO();
                    l.ID = item.ID;
                    l.Username = item.Username;
                    l.Fullname = item.NguoiDung.HoVaTen;
                    data.Add(l);
                }
                return data;
            }

        }
    }
}