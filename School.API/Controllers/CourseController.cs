using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using School.DataTransferObject;
using School.DataTransferObject.Course;
using School.DataTransferObject.TimeTable;
using School.Helpers;
using School.Models;
using School.Services.Main;

namespace School.API.Controllers
{
    public class CourseController : ControllerBase
    {


        private ICourseService _context;
        private IMapper _mapper;
        private ITimeTableService _timeTableService;
        private LinkGenerator _link;

        public CourseController(ICourseService context, IMapper mapper, ITimeTableService timeTableService, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _timeTableService = timeTableService;
            _link = link;
        }

        [HttpGet("/GetAllCourses")]

        public async Task<IActionResult> GetAllCourses(ResourceParameter parameter)
        {
            var courses = await _context.GetAllCourses(parameter);
            var prevLink = courses.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.PreviousPage) : null;
            var nextPage = courses.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.NextPage) : null;
            var pageInfo = new PagingDto
            {
                totalCount = courses.Count,
                pageSize = courses.PageSize,
                totalPages = courses.TotalPages,
                currentPages = courses.CurrentPage,
                PrevLink = prevLink,
                nextLink = nextPage,
            };

            var CourseMapping = new CoursePaging
            {
                Courses = _mapper.Map<IEnumerable<CourseViewDto>>(courses),
                PagingInfo = pageInfo
            };

            return Ok(CourseMapping);
        }



        [HttpGet("/api/GetCourse/{CourseId}")]
        public async Task<IActionResult> GetAllCoursesTimeTable(int CourseId)
        {
            var Exist = await _context.CourseIdExists(CourseId);
            if (Exist == false)
                return BadRequest("Course Doesnt Exist");
            var courses = await _timeTableService.GetTimeTable(CourseId);
            var mapping = _mapper.Map <IEnumerable<TimeTableViewDto>>(courses);
            return Ok(mapping);
        }


        [Authorize(Roles = "Teacher")]
        [HttpPost("/AddCourse")]
        public async Task<IActionResult> AddCourse([FromBody] CourseCreationDto course)
        {
            if (course == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var EntityMapper = _mapper.Map<Course>(course);
            var EntityMapper2 = _mapper.Map<TimeTable>(course);

            var courseList = new List<TimeTable>();
            courseList.Add(EntityMapper2);
            EntityMapper.TimeTables = courseList;

            if(EntityMapper.CourseCredit<1 || EntityMapper.CourseCredit>4)
                return BadRequest("CourseCredit must be between 1 and 4");


            var Exist = await _context.CourseExists(EntityMapper.CourseName);
            if (Exist==true)
                return BadRequest("Course Already Exist");

            var result = await _context.AddCourse(EntityMapper);
            if (result == false)
                return BadRequest("Can't Add the Course");

            return Ok(result);

        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("/AddCourseToTimeTable")]
        public async Task<IActionResult> AddCourseToTimeTable([FromBody] TimeTableCreationDto course)
        {
            if (course == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();


            var EntityMapper = _mapper.Map<TimeTable>(course);

            var Exist = await _context.CourseIdExists(EntityMapper.CourseId);
            if (Exist == false)
                return BadRequest("Course Doesnt Exist!!");

            var timeTables = await _timeTableService.GetTimeTable(EntityMapper.CourseId);
            

            foreach (var timetable in timeTables)
            {
                if ((course.Day == timetable.Day) && ((course.StartTime >= timetable.StartTime) && (course.StartTime <= timetable.EndTime))            
                    || ((course.Day == timetable.Day) && (course.StartTime <= timetable.StartTime) && (course.StartTime >= timetable.EndTime))
                    || ((course.Day == timetable.Day) && (course.EndTime >= timetable.StartTime) && (course.EndTime <= timetable.EndTime))
                    || ((course.Day == timetable.Day) && (course.EndTime <= timetable.StartTime) && (course.EndTime >= timetable.EndTime)))

                    return BadRequest("Has Clash");
            }

            var result = await _timeTableService.AddTimeTable(EntityMapper);

            if (result == false)
                return BadRequest("Cant add the course for user");

            return Ok(result);

        }

        [Authorize(Roles = "Teacher")]
        [HttpPut("updatecourse/{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, [FromBody] CourseUpdateDto course)
        {

            if (course == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var courseFromDb = await _context.GetCourseById(courseId);

            if (courseFromDb == null)
                return BadRequest("This Course Doesn't Exist");

            courseFromDb.CourseName = course.CourseName;
            courseFromDb.CourseDescription = course.CourseDescription;
            courseFromDb.EndDate = course.EndDate;
            courseFromDb.StartDate = course.StartDate;

            var result = await _context.EditCourse(courseFromDb);

            if (result == false)
                return BadRequest();

            return Ok(course);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPut("updateTimetable/{TimeTableId}")]
        public async Task<IActionResult> UpdateCourseTimeTable(int TimeTableId, [FromBody] TimeTableUpdateDto course)
        {

            if (course == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var courseFromDb = await _timeTableService.GetTimeTableById(TimeTableId);
            if (courseFromDb == null)
                return BadRequest("This TimeTable doesnt exist!!");

            var timeTable = await _timeTableService.GetTimeTable(courseFromDb.CourseId);

            foreach (var st in timeTable)
            {
                if ((course.Day == st.Day) && ((course.StartTime >= st.StartTime) && (course.StartTime <= st.EndTime))
                    || ((course.Day == st.Day) && (course.StartTime <= st.StartTime) && (course.StartTime >= st.EndTime))
                    || ((course.Day == st.Day) && (course.EndTime >= st.StartTime) && (course.EndTime <= st.EndTime))
                    || ((course.Day == st.Day) && (course.EndTime <= st.StartTime) && (course.EndTime >= st.EndTime)))

                    return BadRequest("Has Clash");
            }

            courseFromDb.EndTime = course.EndTime;
            courseFromDb.StartTime = course.StartTime;
            courseFromDb.Day = course.Day;



                var result = await _timeTableService.EditTimeTable(courseFromDb);

            if (result == false)
                return BadRequest();

            return Ok("Timetable Updated");
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("deletecourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {

            if (!ModelState.IsValid)
                return BadRequest();


            var result = await _context.GetCourseById(courseId);
            var Exist = await _context.CourseIdExists(courseId);
            if (Exist == false)
                return BadRequest("Course Doesnt Exist");


            await _context.RemoveCourse(result);

            return Ok("Course Deleted");
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("deleteTimeTable{TimeTableId}")]
        public async Task<IActionResult> DeleteCourseTimeTable(int TimeTableId)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var Exist = await _timeTableService.TimeTableExists(TimeTableId);
            if (Exist == false)
                return BadRequest("Timetable doesnt exist!!");
            var result = await _timeTableService.GetTimeTableById(TimeTableId);

           

            if (result == null)
                return BadRequest("Enter the TimeTable Id");

            await _timeTableService.RemoveTimeTable(result);

            return Ok("TimeTable Deleted");
        }


        private string CreateTestListResourceUri(ResourceParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _link.GetPathByAction(HttpContext, "GetAllCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber - 1,
                            PageSize = parameter.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return _link.GetPathByAction(HttpContext, "GetAllCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber + 1,
                            PageSize = parameter.PageSize,
                        });
                case ResourceUriType.Current:
                    return _link.GetPathByAction(HttpContext, "GetAllCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });
                default:
                    return _link.GetPathByAction(HttpContext, "GetAllCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });

            }
        }
}
    }

