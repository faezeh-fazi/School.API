using System;
using System.Collections.Generic;
using System.Text;

namespace School.Models
{
   public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Course> Courses { get; set; }

        public virtual ICollection<User> Users { get; set; }

    }
}
