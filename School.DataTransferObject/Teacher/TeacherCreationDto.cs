using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Teacher
{
   public class TeacherCreationDto
    {
        public string TeacherName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }


    }
}
