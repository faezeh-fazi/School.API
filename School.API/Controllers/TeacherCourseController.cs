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
using School.DataTransferObject.StudentCourse;
using School.DataTransferObject.StudentGrade;
using School.DataTransferObject.Teacher;
using School.DataTransferObject.TeacherCourse;
using School.DataTransferObject.TeacherTimeTable;
using School.Extensions;
using School.Helpers;
using School.Models;
using School.Services.Main;

namespace School.API.Controllers
{
    public class TeacherCourseController : ControllerBase
    {

        private ITeacherCourseService _context;
        private IMapper _mapper;
        private IUserService _userService;
        private ITimeTableService _timeTableService;
        private IStudentCourseService _studentCourseService;
        private LinkGenerator _link;

        public TeacherCourseController(IStudentCourseService studentCourseService, ITeacherCourseService context,
            IMapper mapper, IUserService userService, ITimeTableService timeTableService, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _timeTableService = timeTableService;
            _studentCourseService = studentCourseService;
            _link = link;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("allTeacherCourses/{UserId}")]
        public async Task<IActionResult> GetAllTeacherCourses(string UserId, ResourceParameter parameter)
        {
            if (UserId != HttpContext.GetUserId())
                return BadRequest("Not Allowed!");

            var courses = await _context.GetAllTeacherCourses(parameter, UserId);
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

            var Mapping = new TeacherCoursePaging
            {
                TeacherCourses = _mapper.Map<IEnumerable<TeacherCourseViewDto>>(courses),
                PagingInfo = pageInfo
            };
            return Ok(Mapping);

        }

        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet("TeacherTimeTable/{UserId}")]
        public async Task<IActionResult> GetTeacherTimeTable(string UserId)
        {
            if (UserId == HttpContext.GetUserId() || HttpContext.User.IsInRole("Admin"))
            {
                var courses = await _timeTableService.GetTeacherTimeTable(UserId);

                var mapping = _mapper.Map<IEnumerable<TeacherTimetableViewDto>>(courses);

                return Ok(mapping);
            }
            else
            {
                return BadRequest("Not Allowed!");
            }
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("/api/GetTeacherCourse/{CourseId}")]
        public async Task<IActionResult> GetTeacherCourseById(int CourseId)
        {
            var course = await _context.GetTeacherCourseById(CourseId);
            var Exist = await _context.CourseExists(CourseId, HttpContext.GetUserId());
            if (Exist == false)
                return BadRequest("This Course doesnt exist in your course list!");
            var mapping = _mapper.Map<TeacherCourseViewDto>(course);
            return Ok(mapping);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost("/api/Add/TeacherCourse")]
        public async Task<IActionResult> AddTeacherCourse([FromBody] TeacherCourseCreationDto course)
        {
            if (course == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            if (course.UserId != HttpContext.GetUserId())
                return BadRequest("Not Allow to Add Course for another Teacher!");
            else
            {

                var EntityMapper = _mapper.Map<TeacherCourse>(course);
                var teachertimeTables = await _timeTableService.GetTeacherTimeTable(EntityMapper.UserId);
                var timeTables = await _timeTableService.GetTimeTable(EntityMapper.CourseId);

                foreach (var st in teachertimeTables)
                {
                    foreach (var cs in timeTables)
                    {
                        if ((cs.Day == st.Day) && ((cs.StartTime >= st.StartTime) && (cs.StartTime <= st.EndTime))
                        || ((cs.Day == st.Day) && (cs.StartTime <= st.StartTime) && (cs.StartTime >= st.EndTime))
                        || ((cs.Day == st.Day) && (cs.EndTime >= st.StartTime) && (cs.EndTime <= st.EndTime))
                        || ((cs.Day == st.Day) && (cs.EndTime <= st.StartTime) && (cs.EndTime >= st.EndTime)))

                            return BadRequest("Has Clash");
                    }
                }
                var Exist = await _context.CourseExists(EntityMapper.CourseId, HttpContext.GetUserId());
                if (Exist == true)
                    return BadRequest("Teacher Already has the course");

                var result = await _context.AddTeacherCourse(EntityMapper);
                if (result == false)
                    return BadRequest("Cant add the course for user");

                return Ok(result);

            }
        }
        [Authorize(Roles = "Teacher")]
        [HttpPut("/api/Add/StudentGrade/")]
        public async Task<IActionResult> AddStudentGrades(string StudentId, int CourseId, [FromBody] StudentGradeUpdateDto grade)
        {

            if (grade == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();


            var TeacherHasCourse = await _context.TeacherHasCourse(CourseId, HttpContext.GetUserId());
            if (TeacherHasCourse == false)

                return BadRequest("Not Allow to Add the Grades");
            else
            {

                var EntityMapper = await _studentCourseService.GetStudentGrade(CourseId, StudentId);
                if (EntityMapper == null)
                    return BadRequest("invalid inputs");

                var Exist = await _studentCourseService.CourseExists(EntityMapper.CourseId, StudentId);
                if (Exist == false)
                    return BadRequest("Course for the student is not available");

                EntityMapper.Midterm = grade.Midterm;
                EntityMapper.Final = grade.Final;
                if (EntityMapper.Midterm > 50 || EntityMapper.Midterm < 0 || EntityMapper.Final > 50 || EntityMapper.Final < 0)
                    return BadRequest("Grades Are Out of 50");
                var total = EntityMapper.Midterm + EntityMapper.Final;
                if (total >= 90)
                    EntityMapper.LetterGrade = "A";
                if (total >= 70 && total < 90)
                    EntityMapper.LetterGrade = "B";
                if (total >= 60 && total < 70)
                    EntityMapper.LetterGrade = "C";
                if (total >= 50 && total < 60)
                    EntityMapper.LetterGrade = "D";
                if (total < 50)
                    EntityMapper.LetterGrade = "F";

                var result = await _studentCourseService.EditStudentCourse(EntityMapper);

                if (result == false)
                    return BadRequest("Cant update the grade for student");

                return Ok(result);
            }

        }



        [Authorize(Roles = "Teacher")]
        [HttpDelete("DeleteTeacherCourse/{courseId}")]
        public async Task<IActionResult> DeleteTeacherCourse(int courseId)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _context.GetTeacherCourseById(courseId);
            if (result == null)
                return BadRequest("Course with this ID Doesn't Exist in your course list");

            await _context.RemoveTeacherCourse(result);

            return Ok("Course Deleted");
        }

        private string CreateTestListResourceUri(ResourceParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _link.GetPathByAction(HttpContext, "GetAllTeacherCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber - 1,
                            PageSize = parameter.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return _link.GetPathByAction(HttpContext, "GetAllTeacherCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber + 1,
                            PageSize = parameter.PageSize,
                        });
                case ResourceUriType.Current:
                    return _link.GetPathByAction(HttpContext, "GetAllTeacherCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });
                default:
                    return _link.GetPathByAction(HttpContext, "GetAllTeacherCourses",
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
