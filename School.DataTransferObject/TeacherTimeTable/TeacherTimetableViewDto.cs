using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.TeacherTimeTable
{
   public class TeacherTimetableViewDto
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseId { get; set; }

    }
}
