using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.TeacherCourse
{
   public class TeacherCoursePaging
    {
        public IEnumerable<TeacherCourseViewDto> TeacherCourses { get; set; }
        public PagingDto PagingInfo { get; set; }
    }
}
