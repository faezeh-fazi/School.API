using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using School.Models;

namespace School.DataTransferObject.Course
{
   public class CourseCreationDto
    {

        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public int CourseCredit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }


        public int DepartmentId { get; set; }



    }
}
