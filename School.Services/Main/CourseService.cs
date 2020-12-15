using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.Helpers;
using School.Models;

namespace School.Services.Main
{
    public class CourseService : ICourseService
    {
        private DiscDbContext _context;
        public CourseService(DiscDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Course>> GetAllCourses(ResourceParameter parameter)
        {
           var courses= _context
                .Course
                .Include(x=>x.TimeTables)
                .Include(x=>x.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(parameter.NameFilter))
            {
                courses = courses.Where(x => x.CourseName.Contains(parameter.NameFilter));
            }
            return await PagedList<Course>.CreateAsync(courses, parameter.PageNumber, parameter.PageSize);
        }


        public async Task<Course> GetCourseById(int CourseId)
        {
            return await _context
                .Course
                .Include(x=>x.TimeTables)
                .Include(x => x.Department)

                .FirstOrDefaultAsync(x => x.Id == CourseId);
        }

        public async Task<bool> AddCourse(Course course)
        {
            await _context.Course.AddAsync(course);
            return await SaveAll();
        }
        public async Task<bool> EditCourse(Course course)
        {
            _context.Course.Update(course);
            return await SaveAll();
        }

        public async Task<bool> RemoveCourse(Course course)
        {
            _context.Course.Remove(course);
            return await SaveAll();
        }
        public async Task<bool> CourseExists(string Name)
        {
            if (await _context.Course.AnyAsync(x => x.CourseName == Name))
                return true;

            return false;
        }

        public async Task<bool> CourseIdExists(int Id)
        {
            if (await _context.Course.AnyAsync(x => x.Id == Id))
                return true;

            return false;
        }

        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
