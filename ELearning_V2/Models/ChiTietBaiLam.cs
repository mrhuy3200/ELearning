//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ELearning_V2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietBaiLam
    {
        public Nullable<int> TraLoi { get; set; }
        public int Diem { get; set; }
        public long MaBaiLam { get; set; }
        public long MaCauHoi { get; set; }
    
        public virtual BaiLam BaiLam { get; set; }
        public virtual CauHoi CauHoi { get; set; }
    }
}
