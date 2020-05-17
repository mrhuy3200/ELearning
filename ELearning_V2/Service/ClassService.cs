using ELearning_V2.DTO;
using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
                    l.UserID = item.UserID;
                    data.Add(l);
                }
                return data;
            }
        }
        public static int RemoveCourse(Course c)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    db.Courses.Remove(db.Courses.Find(c.ID));
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                //return 0;
                throw;
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
                    l.Email = item.NguoiDung.Email;
                    l.Image = item.NguoiDung.Image;
                    data.Add(l);
                }
                return data;
            }
        }
        public static List<LessionDTO> GetLessionByUserID(long UserID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lst = db.Lessions.Where(x => x.UserID == UserID).ToList();
                List<LessionDTO> data = new List<LessionDTO>();
                foreach (var item in lst)
                {
                    LessionDTO l = new LessionDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.Content = item.Content;
                    l.URL = item.URL;
                    l.Status = item.Status;
                    l.View = item.LessionViews.Count();
                    data.Add(l);
                }
                return data;
            }
        }
        public static List<LessionDTO> GetLessionByCourseID(long CourseID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstLesID = db.Course_Lession.Where(x => x.CourseID == CourseID).Select(x => x.LessionID).ToList();
                var lst = db.Lessions.Where(x => lstLesID.Contains(x.ID)).ToList();
                List<LessionDTO> data = new List<LessionDTO>();
                foreach (var item in lst)
                {
                    LessionDTO l = new LessionDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.Content = item.Content;
                    l.URL = item.URL;
                    l.Status = item.Status;
                    l.View = item.LessionViews.Count();
                    l.Course_LessionStatus = item.Course_Lession.Where(x => x.LessionID == item.ID && x.CourseID == CourseID).FirstOrDefault().Status;

                    data.Add(l);
                }
                return data;
            }
        }
        public static List<LessionDTO> LoadLessionToAdd(long CourseID, long UserID)
        {
            using (ELearningDB db = new ELearningDB())
            {
                var lstLesID = db.Course_Lession.Where(x => x.CourseID == CourseID).Select(x => x.LessionID).ToList();
                var lst = db.Lessions.Where(x => !lstLesID.Contains(x.ID) && x.UserID == UserID && x.Status == 1).ToList();
                List<LessionDTO> data = new List<LessionDTO>();
                foreach (var item in lst)
                {
                    LessionDTO l = new LessionDTO();
                    l.ID = item.ID;
                    l.Name = item.Name;
                    l.Content = item.Content;
                    l.URL = item.URL;
                    l.Status = item.Status;
                    l.View = item.LessionViews.Count();
                    data.Add(l);
                }
                return data;

            }
        }
        public static bool EditCourse(CourseDTO c)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var cou = db.Courses.Find(c.ID);
                    cou.Name = c.Name;
                    cou.Price = c.Price;
                    cou.Schedule = c.Schedule;
                    cou.Description = c.Description;
                    cou.Condition = c.Condition;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static int AddMember(CourseDetail cd)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var check = db.CourseDetails.Where(x => x.UserID == cd.UserID && x.CourseID == cd.CourseID).FirstOrDefault();
                    if (check != null)
                    {
                        return -1;
                    }
                    db.CourseDetails.Add(cd);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int RemoveMember(CourseDetail cd)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var check = db.CourseDetails.Where(x => x.UserID == cd.UserID && x.CourseID == cd.CourseID).FirstOrDefault();
                    if (check == null)
                    {
                        return -1;
                    }
                    db.CourseDetails.Remove(check);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int ChangeStatusLession(LessionDTO l)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    if (l.CourseID == null)
                    {
                        Lession data = db.Lessions.Find(l.ID);
                        data.Status = l.Status;
                        db.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        var data = db.Course_Lession.Where(x => x.LessionID == l.ID && x.CourseID == l.CourseID).FirstOrDefault();
                        data.Status = l.Course_LessionStatus;
                        db.SaveChanges();
                        return 1;
                    }

                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static int ChangeStatusCourse(CourseDTO c)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Course data = db.Courses.Find(c.ID);
                    data.Status = c.Status;
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static int ChangeStatusQuestion(QuestionDTO q)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Question data = db.Questions.Find(q.ID);
                    data.Status = q.Status;
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
                //return 0;
                //throw;
            }
        }
        public static LessionDTO AddLessionToCourse(long CourseID, long LessionID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Course_Lession data = new Course_Lession();
                    data.CourseID = CourseID;
                    data.LessionID = LessionID;
                    data.Status = 2;
                    db.Course_Lession.Add(data);
                    db.SaveChanges();
                    return GetLessionByID(LessionID);
                }
            }
            catch (Exception)
            {
                //return null;
                throw;
            }
        }
        public static LessionDTO RemoveLessionFromCourse(long CourseID, long LessionID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var data = db.Course_Lession.Where(x => x.CourseID == CourseID && x.LessionID == LessionID).FirstOrDefault();
                    db.Course_Lession.Remove(data);
                    db.SaveChanges();
                    return GetLessionByID(LessionID);
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public static int RemoveLession(LessionDTO l)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Lession data = db.Lessions.Find(l.ID);
                    var lstCmt = db.Comments.Where(x => x.LessionID == l.ID).ToList();
                    foreach (var item in lstCmt)
                    {
                        var lstRep = db.Replies.Where(x => x.CommentID == item.ID).ToList();
                        foreach (var rep in lstRep)
                        {
                            db.Replies.Remove(rep);
                        }
                        db.Comments.Remove(item);
                    }
                    db.Lessions.Remove(data);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static LessionDTO GetLessionByID(long LessionID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var l = db.Lessions.Find(LessionID);
                    var lstCmtID = db.Comments.Where(x => x.LessionID == LessionID).Select(a => a.ID);
                    var lstRep = db.Replies.Where(x => lstCmtID.Contains(x.CommentID)).ToList();
                    var lstTopic = db.Lession_Topic.Where(x => x.LessionID == LessionID).ToList();
                    List<TopicDTO> lstTopicDTO = new List<TopicDTO>();
                    foreach (var item in lstTopic)
                    {
                        TopicDTO t = new TopicDTO();
                        t.ID = (long)item.TopicID;
                        t.Name = item.Topic.Name;
                        lstTopicDTO.Add(t);
                    }
                    LessionDTO data = new LessionDTO();
                    data.ID = l.ID;
                    data.Name = l.Name;
                    data.Content = l.Content;
                    data.URL = l.URL;
                    data.Status = l.Status;
                    data.UserID = l.UserID;
                    data.View = l.LessionViews.Count();
                    data.Comment = l.Comments.Count() + lstRep.Count();
                    data.CreateDate = (DateTime)l.CreateDate;
                    data.Image = l.Image;
                    data.Username = l.NguoiDung.HoVaTen;
                    data.UserAvatar = l.NguoiDung.Image;
                    data.UserInfo = l.NguoiDung.Info;
                    data.Topics = lstTopicDTO;
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public static List<CommentDTO> GetCommentByID(CommentDTO c, int flag)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    List<Comment> lstCom = new List<Comment>();
                    if (flag == 1)
                    {
                        lstCom = db.Comments.Where(x => x.ClassID == c.ClassID).ToList();
                    }
                    if (flag == 2)
                    {
                        lstCom = db.Comments.Where(x => x.CourseID == c.CourseID && x.LessionID == null).ToList();
                    }
                    if (flag == 3)
                    {
                        lstCom = db.Comments.Where(x => x.LessionID == c.LessionID && x.CourseID == c.CourseID).ToList();
                    }
                    List<CommentDTO> Coms = new List<CommentDTO>();
                    foreach (var item in lstCom)
                    {
                        CommentDTO com = new CommentDTO();
                        com.ID = item.ID;
                        com.NoiDung = item.NoiDung;
                        com.CreateDate = item.CreateDate;
                        com.CreateBy = item.CreateBy;
                        if (item.Rate != null)
                        {
                            com.Rate = (int)item.Rate;
                        }
                        if (item.TaiKhoan.Role == 4)
                        {
                            com.Fullname = item.TaiKhoan.NguoiDung.HoVaTen;
                        }
                        if (item.TaiKhoan.Role == 3)
                        {
                            com.Fullname = item.TaiKhoan.HocVien.HoVaTen;
                        }
                        if (item.TaiKhoan.Role == 2)
                        {
                            com.Fullname = item.TaiKhoan.GiangVien.HoVaTen;
                        }
                        List<Reply> lstRep = db.Replies.Where(x => x.CommentID == item.ID).ToList();
                        List<CommentDTO> reps = new List<CommentDTO>();
                        foreach (var rep in lstRep)
                        {
                            CommentDTO r = new CommentDTO();
                            r.ID = rep.ID;
                            r.NoiDung = rep.NoiDung;
                            r.CreateDate = rep.CreateDate;
                            r.CreateBy = rep.CreateBy;
                            if (rep.TaiKhoan.Role == 4)
                            {
                                r.Fullname = rep.TaiKhoan.NguoiDung.HoVaTen;
                            }
                            if (rep.TaiKhoan.Role == 3)
                            {
                                r.Fullname = rep.TaiKhoan.HocVien.HoVaTen;
                            }
                            if (rep.TaiKhoan.Role == 2)
                            {
                                r.Fullname = rep.TaiKhoan.GiangVien.HoVaTen;
                            }
                            reps.Add(r);
                        }
                        com.Replies = reps;
                        Coms.Add(com);
                    }
                    return Coms;
                }

            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public static long CreateLession(LessionDTO l)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Lession data = new Lession();
                    data.Name = l.Name;
                    data.Content = l.Content;
                    data.URL = l.URL;
                    data.UserID = l.UserID;
                    data.Status = 2;
                    data.CreateDate = DateTime.Now;
                    db.Lessions.Add(data);
                    db.SaveChanges();
                    long id = db.Lessions.OrderByDescending(p => p.ID).FirstOrDefault().ID;
                    data.Image = id + ".jpg";
                    db.SaveChanges();
                    return id;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static long EditLession(LessionDTO l)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    Lession data = db.Lessions.Find(l.ID);
                    data.Name = l.Name;
                    data.Content = l.Content;
                    data.URL = l.URL;
                    data.Status = l.Status;
                    db.SaveChanges();
                    return data.ID;
                }

            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static List<QuestionDTO> GetListQuestionByUserID(long UserID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    List<QuestionDTO> data = new List<QuestionDTO>();
                    var lst = db.Questions.Where(x => x.UserID == UserID).ToList();
                    foreach (var item in lst)
                    {
                        QuestionDTO d = new QuestionDTO();
                        d.ID = item.ID;
                        d.Content = item.Content;
                        d.AnswerID = item.AnswerID;
                        d.Image = item.Image;
                        d.Status = item.Status;
                        d.CreateDate = item.CreateDate;
                        d.Level = item.Level;
                        d.UserID = item.UserID;
                        var lstAns = db.Answers.Where(x => x.QuesionID == item.ID);
                        List<AnswerDTO> Adata = new List<AnswerDTO>();
                        foreach (var a in lstAns)
                        {
                            AnswerDTO asw = new AnswerDTO();
                            asw.ID = a.ID;
                            asw.Content = a.Content;
                            asw.Image = a.Image;
                            asw.QuesionID = a.QuesionID;
                            Adata.Add(asw);
                        }
                        d.Answers = Adata;
                        data.Add(d);
                    }
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public static long CreateQuestion(Question q)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    db.Questions.Add(q);
                    db.SaveChanges();
                    return db.Questions.OrderByDescending(p => p.ID).FirstOrDefault().ID;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static long CreateAnswer(Answer a)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    db.Answers.Add(a);
                    db.SaveChanges();
                    return db.Answers.OrderByDescending(p => p.ID).FirstOrDefault().ID;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        public static bool CreateQuestionTopic(long QuestionID, List<TopicDTO> Topics)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    foreach (var item in Topics)
                    {
                        Question_Topic t = new Question_Topic();
                        t.QuestionID = QuestionID;
                        t.TopicID = item.ID;
                        db.Question_Topic.Add(t);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }    
        public static bool EditQuestion(QuestionDTO q)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var Qdata = db.Questions.Find(q.ID);
                    List<Answer> removelist = new List<Answer>();
                    Qdata.Content = q.Content;
                    Qdata.Solution = q.Solution;
                    Qdata.Level = q.Level;
                    db.SaveChanges();
                    var TopicDelete = db.Question_Topic.Where(x => x.QuestionID == q.ID).ToList();
                    db.Question_Topic.RemoveRange(TopicDelete);
                    foreach (var item in q.Topics)
                    {
                        Question_Topic t = new Question_Topic();
                        t.QuestionID = q.ID;
                        t.TopicID = item.ID;
                        db.Question_Topic.Add(t);
                    }
                    db.SaveChanges();
                    var Adata = db.Answers.Where(x => x.QuesionID == Qdata.ID).ToList();
                    for (int i = 0; i < q.Answers.Count; i++)
                    {
                        long AnswerID;
                        if (q.Answers[i].ID != 0)
                        {
                            var temp = Adata.Where(a => a.ID == q.Answers[i].ID).FirstOrDefault();
                            temp.Content = q.Answers[i].Content;
                            AnswerID = temp.ID;
                            db.SaveChanges();
                        }
                        else
                        {
                            Answer newAns = new Answer();
                            newAns.Content = q.Answers[i].Content;
                            newAns.QuesionID = q.ID;
                            db.Answers.Add(newAns);
                            db.SaveChanges();
                            AnswerID = db.Answers.OrderByDescending(x => x.ID).FirstOrDefault().ID;
                        }
                        if (q.AnswerID == i)
                        {
                            Qdata.AnswerID = AnswerID;
                            db.SaveChanges();
                        }
                    }
                    var AdataID = Adata.Select(x => x.ID).ToList();
                    var AnswersID = q.Answers.Where(x => x.ID != 0).Select(x => x.ID);
                    for (int i = 0; i < AdataID.Count; i++)
                    {
                        if (AnswersID.Contains(AdataID[i]) == false)
                        {
                            db.Answers.Remove(Adata[i]);
                            db.SaveChanges();
                        }
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public static bool RemoveQuestion(QuestionDTO q)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var data = db.Questions.Find(q.ID);
                    if (data == null)
                    {
                        return false;
                    }
                    db.Questions.Remove(data);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public static QuestionDTO GetQuestion(long QuestionID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    QuestionDTO data = new QuestionDTO();
                    var q = db.Questions.Find(QuestionID);
                    if (q != null)
                    {
                        data.ID = q.ID;
                        data.Content = q.Content;
                        data.Solution = q.Solution;
                        data.AnswerID = q.AnswerID;
                        data.Level = q.Level;
                        data.UserID = q.UserID;
                        List<TopicDTO> Tdata = new List<TopicDTO>();
                        foreach (var item in q.Question_Topic)
                        {
                            TopicDTO t = new TopicDTO();
                            t.ID = item.ID;
                            Tdata.Add(t);
                        }
                        data.Topics = Tdata;
                        List<AnswerDTO> Adata = new List<AnswerDTO>();
                        foreach (var item in q.Answers)
                        {
                            AnswerDTO a = new AnswerDTO();
                            a.ID = item.ID;
                            a.Content = item.Content;
                            Adata.Add(a);
                        }
                        data.Answers = Adata;
                        return data;
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public static List<TestDTO> GetListTestByCourseID(long CourseID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var lstTest = db.Tests.Where(x => x.CourseID == CourseID).ToList();
                    List<TestDTO> data = new List<TestDTO>();
                    foreach (var item in lstTest)
                    {
                        TestDTO t = new TestDTO();
                        t.ID = item.ID;
                        t.Name = item.Name;
                        t.CreateDate = item.CreateDate;
                        t.Status = item.Status;
                        t.CourseID = item.CourseID;
                        data.Add(t);
                    }
                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List<TopicDTO> GetListTopicByUserID(long UserID)
        {
            try
            {
                using (ELearningDB db = new ELearningDB())
                {
                    var lst = db.Topics.Where(x => x.UserID == UserID).ToList();
                    List<TopicDTO> data = new List<TopicDTO>();
                    foreach (var item in lst)
                    {
                        TopicDTO t = new TopicDTO();
                        t.ID = item.ID;
                        t.Name = item.Name;
                        data.Add(t);
                    }
                    return data;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }    
    }
}