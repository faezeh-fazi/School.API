using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Course
{
    public class CoursePaging
    {
        public IEnumerable<CourseViewDto> Courses { get; set; }

        public PagingDto PagingInfo { get; set; }

    }
}
