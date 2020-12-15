using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataTransferObject
{
   public class PagingDto
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int currentPages { get; set; }
        public string PrevLink { get; set; }
        public string nextLink { get; set; }
    }
}
