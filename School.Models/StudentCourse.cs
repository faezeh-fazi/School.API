using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace School.Models
{
   public class StudentCourse
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }

        public float Midterm { get; set; }
      
        public float Final { get; set; }
        public string LetterGrade { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
      
    }

  
  
}
