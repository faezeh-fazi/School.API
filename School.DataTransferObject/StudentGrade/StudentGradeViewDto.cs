using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.StudentGrade
{
   public class StudentGradeViewDto
    {
        public string CourseName { get; set; }
        public float Midterm { get; set; }
        public float Final { get; set; }

        public string LetterGrade { get; set; }

    }
}
