using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Teacher
{
   public class TeacherPaging
    {
        public IEnumerable<TeacherViewDto> Teachers { get; set; }

        public PagingDto PagingInfo { get; set; }
    }
}
