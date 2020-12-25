using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.StudentTimeTable
{
    public class StudentTimetableViewDto
    {
        public string courseId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string Day { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

    }
}
