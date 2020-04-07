using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearning_V2.Areas.GV.Models
{
    public class LopModel
    {
        public int MaLop { get; set; }
        public string TenLop { get; set; }
        public bool TrangThai { get; set; }
        public long MaGiangVien { get; set; }
        public int MaMonHoc { get; set; }
        public string NgayBatDau { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
        public DateTime StartDate { get; set; }
        public string NgayKetThuc { get; set; }
        public string Image { get; set; }
        public int? SiSo { get; set; }
        public string MoTa { get; set; }

        public string HoTenGV { get; set; }
        public string TenMonHoc { get; set; }
        





    }
}