using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearning_V2.common
{
    public class CauHoiModel
    {
        public long MaCauHoi { get; set; }

        [Required(ErrorMessage = "Nội dung không được trống")]
        [MaxLength(150, ErrorMessage = "Tối đa 150 ký tự")]
        public string NoiDung { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        [AllowHtml]
        public string BieuThuc { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string CauA { get; set; }

        [MaxLength(30, ErrorMessage = "Tối đa 30 ký tự")]
        public string BieuThucA { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string CauB { get; set; }

        [MaxLength(30, ErrorMessage = "Tối đa 30 ký tự")]
        public string BieuThucB { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string CauC { get; set; }

        [MaxLength(30, ErrorMessage = "Tối đa 30 ký tự")]
        public string BieuThucC { get; set; }

        [MaxLength(100, ErrorMessage = "Tối đa 100 ký tự")]
        public string CauD { get; set; }

        [MaxLength(30, ErrorMessage = "Tối đa 30 ký tự")]
        public string BieuThucD { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đáp án")]
        public int DapAn { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public double Diem { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        public int DoKho { get; set; }

        public bool TrangThai { get; set; }

        public string NgayTao { get; set; }

        public string NgaySua { get; set; }

        public string Image { get; set; }

        public int MaMonHoc { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chương")]
        public long ChuongID { get; set; }

        public string TenChuong { get; set; }

        public long GiangVienID { get; set; }

        public string TenGV { get; set; }


    }
}