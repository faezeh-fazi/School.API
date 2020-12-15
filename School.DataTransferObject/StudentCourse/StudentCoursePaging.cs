using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.StudentCourse
{
   public class StudentCoursePaging
    {
        public IEnumerable<StudentCourseViewDto> StudentCourses { get; set; }
        public PagingDto PagingInfo { get; set; }
    }
}
