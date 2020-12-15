
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.Helpers;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace School.Services.Main
{
    public class StudentCourseService : IStudentCourseService
    {
        private DiscDbContext _context;
        public StudentCourseService(DiscDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<StudentCourse>> GetAllStudentCourses(ResourceParameter parameter, string StudentId)
        {

            var studentCourse = _context
                 .StudentCourse
                 .Include(x => x.Course)
                 .ThenInclude(x => x.TimeTables)
                 .Where(x => x.UserId == StudentId)
                 .AsQueryable();

            if (!string.IsNullOrEmpty(parameter.NameFilter))
            {
                studentCourse = studentCourse.Where(x => x.Course.CourseName.Contains(parameter.NameFilter));
            }
            return await PagedList<StudentCourse>.CreateAsync(studentCourse, parameter.PageNumber, parameter.PageSize);
        }

        public async Task<StudentCourse> GetStudentCourseById(int CourseId,string StudentId)
        {
           
            return await _context
                .StudentCourse
                .Include(x=>x.Course)
                .Where(x=>x.UserId==StudentId)
                .FirstOrDefaultAsync(x => x.CourseId == CourseId);

        }
        public async Task<StudentCourse> GetStudentGrade(int CourseId, string StudentId)
        {

            return await _context
                .StudentCourse
                .Include(x => x.Course)
                .Where(x=>x.UserId==StudentId)
                .FirstOrDefaultAsync(x => x.CourseId == CourseId);

        }


        /*   public async Task<StudentCourse> GetStudentTranscript(string StudentId)
           {
               var courses = await _context
                    .StudentCourse
                    .Include(x => x.Course)
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.UserId == StudentId);

               return courses;
           }*/


        public async Task<User> GetStudentTranscript(string StudentId)
        {
            var courses = await _context
                 .User
                 .Include(x => x.StudentCourses)
                 .FirstOrDefaultAsync(x => x.Id == StudentId);

            return courses;
        }

        public async Task<bool> AddStudentCourse(StudentCourse course)
        {

            await _context.StudentCourse.AddAsync(course);
  
            return await SaveAll();
        }
        public async Task<bool> EditStudentCourse(StudentCourse course)
        {
            _context.StudentCourse.Update(course);
            return await SaveAll();
        }

        public async Task<bool> RemoveStudentCourse(StudentCourse course)
        {
            _context.StudentCourse.Remove(course);
            return await SaveAll();
        }

        public async Task<bool> CourseExists(int courseId, string userId)
        {
            if (await _context.StudentCourse.Where(x=>x.UserId==userId).AnyAsync(x => x.CourseId == courseId))
                return true;

            return false;
        }

        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

      


    }
}
