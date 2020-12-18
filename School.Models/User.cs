using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace School.Models
{
    public class User : IdentityUser
    {

        public string Name { get; set; }
        public double GPA { get; set; }
        public string Photo { get; set; }

        public int? DepartmentId { get; set; }

        //[ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public virtual List<StudentCourse> StudentCourses { get; set;}
        public virtual List<TeacherCourse> TeacherCourses { get; set; }


        public bool IsDeleted { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsActive { get; set; }

    }
}
