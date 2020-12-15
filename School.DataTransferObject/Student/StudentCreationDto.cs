using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Student
{
   public class StudentCreationDto
    {
        public string UserName { get; set; }
        public string StudentName { get; set; }
        public string Password { get; set; }
        public int DepartmentId { get; set; }
        public string Role { get; set; }



    }

}
