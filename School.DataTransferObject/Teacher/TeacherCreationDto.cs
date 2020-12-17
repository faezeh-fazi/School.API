using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace School.DataTransferObject.Teacher
{
   public class TeacherCreationDto
    {
        public string TeacherName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        [EmailAddress]
        public string Email { get; set; }


    }
}
