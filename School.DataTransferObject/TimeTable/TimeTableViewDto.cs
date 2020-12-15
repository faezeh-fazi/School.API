using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.TimeTable
{
    public class TimeTableViewDto
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Day { get; set; }
    }
}
