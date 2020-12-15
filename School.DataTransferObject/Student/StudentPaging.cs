using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Student
{
   public class StudentPaging
    {
        public IEnumerable<StudentViewDto> Students { get; set; }

        public PagingDto PagingInfo { get; set; }
    }
}
