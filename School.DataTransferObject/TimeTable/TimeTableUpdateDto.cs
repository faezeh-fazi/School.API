using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.TimeTable
{
   public class TimeTableUpdateDto
    {

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }
    }
}
