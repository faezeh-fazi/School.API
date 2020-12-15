
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace School.Services.Main
{
    public class TeacherCourseService : ITeacherCourseService
    {
        private DiscDbContext _context;
        public TeacherCourseService(DiscDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherCourse>> GetAllTeacherCourses(string UserId)
        {
            return await _context
                .TeacherCourse
             .Include(x => x.Course)
                .Where(x => x.UserId == UserId)
                .ToListAsync();
        }

        public async Task<TeacherCourse> GetTeacherCourseById(int CourseId)
        {
            return await _context
                .TeacherCourse
                .Include(x=>x.Course)
                .FirstOrDefaultAsync(x => x.CourseId == CourseId);
        }

        public async Task<bool> AddTeacherCourse(TeacherCourse course)
        {
            await _context.TeacherCourse.AddAsync(course);
            return await SaveAll();
        }
        public async Task<bool> EditTeacherCourse(TeacherCourse course)
        {
            _context.TeacherCourse.Update(course);
            return await SaveAll();
        }

        public async Task<bool> RemoveTeacherCourse(TeacherCourse course)
        {
            _context.TeacherCourse.Remove(course);
            return await SaveAll();
        }

        public async Task<bool> CourseExists(int courseId, string TeacherId)
        {
            if (await _context.TeacherCourse.Where(x=>x.UserId==TeacherId).AnyAsync(x => x.CourseId == courseId))
                return true;

            return false;
        }

        public async Task<bool> TeacherHasCourse(int courseId, string TeacherId)
        {
            if (await _context.TeacherCourse.Where(x=>x.CourseId==courseId).AnyAsync(x => x.UserId == TeacherId))
                return true;

            return false;
        }


        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
