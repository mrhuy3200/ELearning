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
    
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            this.Answers = new HashSet<Answer>();
            this.Question_Topic = new HashSet<Question_Topic>();
            this.Test_Question = new HashSet<Test_Question>();
            this.TestDetails = new HashSet<TestDetail>();
        }
    
        public long ID { get; set; }
        public string Content { get; set; }
        public Nullable<long> AnswerID { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<int> Level { get; set; }
        public string Solution { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question_Topic> Question_Topic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test_Question> Test_Question { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestDetail> TestDetails { get; set; }
    }
}
