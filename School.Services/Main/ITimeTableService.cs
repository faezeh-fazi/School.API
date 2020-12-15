using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models;

namespace School.Services.Main
{
    public interface ITimeTableService
    {
        Task<bool> AddTimeTable(TimeTable course);
        Task<bool> EditTimeTable(TimeTable course);
        Task<IEnumerable<TimeTable>> GetStudentTimeTable(string UserId);
        Task<IEnumerable<TimeTable>> GetTeacherTimeTable(string UserId);

        Task<IEnumerable<TimeTable>> GetTimeTable(int CourseId);
        Task<TimeTable> GetTimeTableById(int TimeTableId);
        Task<bool> TimeTableExists(int Id);
        Task<bool> RemoveTimeTable(TimeTable course);
    }
}