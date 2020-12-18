using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace School.DataTransferObject.Student
{
   public class StudentUpdateDto
    {
        public string StudentName { get; set; }

        public IFormFile Photo { get; set; }
    }
}
