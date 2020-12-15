using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.Models;

namespace School.Services.Main
{
    public class TimeTableService : ITimeTableService
    {
        private DiscDbContext _context;
        public TimeTableService(DiscDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeTable>> GetTimeTable(int CourseId)
        {
            var courses= await _context
                .TimeTable
                .Include(x => x.Course)
                .ThenInclude(x => x.StudentCourses)
                .Where(x => x.CourseId == CourseId)
                .OrderBy(x => x.Day)
                .ThenBy(x =>x.StartTime)
                .ToListAsync();
            return courses.OrderBy(x => x.Day).ThenBy(x => DateTime.Parse(x.StartTime.ToString("h:mm tt"))).ToList();
        }

        public async Task<IEnumerable<TimeTable>> GetStudentTimeTable(string UserId)
        {
            var Courses = await _context
                    .StudentCourse
                    .Include(x => x.Course)
                    .ThenInclude(x => x.TimeTables)
                   .OrderBy(x => x.Course.Day)
                   .ThenBy(x => x.Course.StartTime)
                    .Where(x => x.UserId == UserId)       
                    .ToListAsync();

            
            var timeTable = new List<TimeTable>();

            foreach (var course in Courses)
            {
                timeTable.AddRange(course.Course.TimeTables); 
  
            }

            return timeTable.OrderBy(x => x.Day).ThenBy(x => DateTime.Parse(x.StartTime.ToString("h:mm tt"))).ToList();

        }

        public async Task<IEnumerable<TimeTable>> GetTeacherTimeTable(string UserId)
        {
            var Courses = await _context
                    .TeacherCourse
                    .Include(x => x.Course)
                    .ThenInclude(x => x.TimeTables)
                   .OrderBy(x => x.Course.Day)
                   .ThenBy(x => x.Course.StartTime)
                    .Where(x => x.UserId == UserId)
                    .ToListAsync();


            var timeTable = new List<TimeTable>();

            foreach (var course in Courses)
            {
                timeTable.AddRange(course.Course.TimeTables);

            }

            return timeTable.OrderBy(x => x.Day).ThenBy(x => DateTime.Parse(x.StartTime.ToString("h:mm tt"))).ToList();

        }




        public async Task<TimeTable> GetTimeTableById(int TimeTableId)
        {
            return await _context
                .TimeTable
                .FirstOrDefaultAsync(x => x.Id == TimeTableId);
        }

        public async Task<bool> AddTimeTable(TimeTable course)
        {
            await _context.TimeTable.AddAsync(course);
            return await SaveAll();
        }
        public async Task<bool> EditTimeTable(TimeTable course)
        {
            _context.TimeTable.Update(course);
            return await SaveAll();
        }

        public async Task<bool> RemoveTimeTable(TimeTable course)
        {
            _context.TimeTable.Remove(course);
            return await SaveAll();
        }
        public async Task<bool> TimeTableExists(int Id)
        {
            if (await _context.TimeTable.AnyAsync(x => x.Id == Id))
                return true;

            return false;
        }

        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

