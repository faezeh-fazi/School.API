using System.Collections.Generic;
using System.Threading.Tasks;
using School.Helpers;
using School.Models;

namespace School.Services.Main
{
    public interface IStudentCourseService
    {
        Task<bool> AddStudentCourse(StudentCourse course);
        Task<bool> EditStudentCourse(StudentCourse course);
        Task<PagedList<StudentCourse>> GetAllStudentCourses(ResourceParameter parameter, string StudentId);
        Task<User> GetStudentTranscript(string StudentId);
        Task<StudentCourse> GetStudentCourseById(int CourseId, string StudentId);
        Task<StudentCourse> GetStudentGrade(int CourseId, string StudentId);
        Task<bool> CourseExists(int courseId, string userId);
        Task<bool> RemoveStudentCourse(StudentCourse course);
    }
}