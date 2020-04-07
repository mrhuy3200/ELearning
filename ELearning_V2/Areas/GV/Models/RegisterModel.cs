using ELearning_V2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class RegisterModel
    {
        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="Vui lòng nhập tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name ="Email")]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name ="Mật khẩu")]
        [StringLength(20,MinimumLength =6,ErrorMessage ="Độ dài mật khẩu từ 9 đến 20 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
}