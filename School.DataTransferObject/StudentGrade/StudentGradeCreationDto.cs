using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.StudentGrade
{
   public class StudentGradeCreationDto
    {
        public string UserId { get; set; }

        public int CourseId { get; set; }
        public float Midterm { get; set; }
        public float Final { get; set; }

    }
}
