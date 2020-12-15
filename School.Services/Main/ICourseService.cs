using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models;

namespace School.Services.Main
{
    public interface ICourseService
    {
        Task<bool> AddCourse(Course course);
        Task<bool> EditCourse(Course course);
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> GetCourseById(int CourseId);
        Task<bool> CourseExists(string Name);
        Task<bool> CourseIdExists(int Id);
        Task<bool> RemoveCourse(Course course);
    }
}