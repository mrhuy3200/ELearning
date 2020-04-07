using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Vui lòng nhập Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password không được để trống")]
        public string PassWord { get; set; }

    }
}