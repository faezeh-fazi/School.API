using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.DataTransferObject.Teacher
{
   public class TeacherViewDto
    {
        public string TeacherName { get; set; }
        [EmailAddress]
        public string Email { get; set; }


    }
}
