using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class HocVienModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        [MaxLength(30, ErrorMessage = "Tối đa 30 ký tự")]
        public string HoVaTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không đúng")]
        public string Email { get; set; }

        public string Image { get; set; }

        public bool GioiTinh { get; set; }

        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại không đúng")]
        public string SDT { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        public string NewPass { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
        public DateTime NgaySinh { get; set; }

        public string BirthDate { get; set; }

    }
}