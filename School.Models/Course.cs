using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Models
{
   public class Course
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int CourseCredit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]

            public Department Department { get; set; }

        public virtual List<StudentCourse> StudentCourses { get; set; }
        public virtual List<TeacherCourse> TeacherCourses { get; set; }
        public virtual List<TimeTable> TimeTables { get; set; }



    }
}
