using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject.Department
{
   public class DepartmentPaging
    {
        public IEnumerable<DepartmentViewDto> Departments { get; set; }
        public PagingDto PagingInfo { get; set; }


    }
}
