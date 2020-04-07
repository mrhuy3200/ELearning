using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.Areas.GV.Models
{
    public class BaiGiangModel
    {
        public long MaBaiGiang { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên bài giảng")]
        public string TenBaiGiang { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "Vui lòng nhập nội dung bài giảng")]
        public string NoiDung { get; set; }
        public long MaGiangVien { get; set; }
        public bool TrangThai { get; set; }
        [RegularExpression(@"^(https?\:\/\/)?(www\.youtube\.com|youtu\.?be)\/.+$",
        ErrorMessage = "Vui lòng chọn URL Youtube.")]
        public string URL { get; set; }
        public int MaLop { get; set; }

        public string TenGiangVien { get; set; }



    }
}