using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ELearning_V2.common
{
    public class ImportExcel
    {
        [Required(ErrorMessage = "Vui lòng chọn file")]
        [FileExt(Allow = ".xls,.xlsx", ErrorMessage = "Chỉ chấp nhận file excel")]
        public HttpPostedFileBase file { get; set; }
    }
}