using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.TimeTable
{
    public class TimeTableCreationDto
    {
        public int CourseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }


    }
}
