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
    
    public partial class Test_Question
    {
        public long ID { get; set; }
        public Nullable<long> TestID { get; set; }
        public Nullable<long> QuestionID { get; set; }
    
        public virtual Question Question { get; set; }
        public virtual Test Test { get; set; }
    }
}
