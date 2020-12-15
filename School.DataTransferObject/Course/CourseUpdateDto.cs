using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Course
{
   public class CourseUpdateDto
    {
        public string CourseName { get; set; }

        public string CourseDescription { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}

