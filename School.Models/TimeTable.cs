using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Models
{
   public class TimeTable
    {

        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }
        public DayOfWeek Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }


    }
}
