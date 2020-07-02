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
    
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            this.Comments = new HashSet<Comment>();
            this.Course_Lession = new HashSet<Course_Lession>();
            this.CourseDetails = new HashSet<CourseDetail>();
            this.Messages = new HashSet<Message>();
            this.Notifications = new HashSet<Notification>();
            this.Tests = new HashSet<Test>();
        }
    
        public long ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Capacity { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<double> Price { get; set; }
        public string Schedule { get; set; }
        public string Condition { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<int> MaMonHoc { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course_Lession> Course_Lession { get; set; }
        public virtual MonHoc MonHoc { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseDetail> CourseDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Messages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Notification> Notifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
