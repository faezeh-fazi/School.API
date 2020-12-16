using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using School.DataTransferObject;
using School.DataTransferObject.Student;
using School.DataTransferObject.StudentCourse;
using School.DataTransferObject.StudentGrade;
using School.DataTransferObject.StudentTimeTable;
using School.Extensions;
using School.Helpers;
using School.Models;
using School.Services.Main;

namespace School.API.Controllers
{
    public class StudentCourseController : ControllerBase
    {
        private IStudentCourseService _context;
        private IMapper _mapper;
        private IUserService _userService;
        private ITimeTableService _timeTableService;
        private LinkGenerator _link;

        public StudentCourseController(IStudentCourseService context, IMapper mapper, IUserService userService, ITimeTableService timeTableService, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _timeTableService = timeTableService;
            _link = link;
        }


        [Authorize(Roles = "Student")]
        [HttpGet("allStudentCourses/{UserId}")]
        public async Task<IActionResult> GetAllStudentCourses(string UserId,ResourceParameter parameter)
        {
            if (UserId != HttpContext.GetUserId())
                return BadRequest("Not Allowed!");

            var courses = await _context.GetAllStudentCourses(parameter, UserId);
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

            var Mapping = new StudentCoursePaging
            {
                StudentCourses = _mapper.Map<IEnumerable<StudentCourseViewDto>>(courses),
                PagingInfo = pageInfo
            };
            return Ok(Mapping);

        }


        [Authorize(Roles = "Student")]
        [HttpGet("StudentTimeTable/{UserId}")]
        public async Task<IActionResult> GetStudentTimeTable(string UserId)
        {
            if (UserId != HttpContext.GetUserId())
                return BadRequest("Not Allowed!");

            var courses = await _timeTableService.GetStudentTimeTable(UserId);

            var mapping = _mapper.Map<IEnumerable<StudentTimetableViewDto>>(courses);
            return Ok(mapping);

        }

        [Authorize(Roles = "Student")]
        [HttpGet("/api/GetStudentCourse/{CourseId}")]
        public async Task<IActionResult> GetStudentCourseById(int CourseId)
        {

            var course = await _context.GetStudentCourseById(CourseId, HttpContext.GetUserId());
            var Exist = await _context.CourseExists(CourseId, HttpContext.GetUserId());
            if (Exist == false)
                return BadRequest("This Course doesnt exist in your course list!");
            var mapping = _mapper.Map<StudentCourseViewDto>(course);
            return Ok(mapping);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("StudentGrades/")]
        public async Task<IActionResult> GetStudentGrades(string UserId,ResourceParameter parameter)
        {
            if (UserId != HttpContext.GetUserId())
                return BadRequest("You are not allow to see the Grades!");
            var courses = await _context.GetAllStudentCourses(parameter, UserId);
            var mapping = _mapper.Map<IEnumerable<StudentGradeViewDto>>(courses);

            return Ok(mapping);
        }

        [Authorize(Roles = "Student")]
        [HttpGet("GetStudentTranscript/")]
        public async Task<IActionResult> GetStudentTranscript(string StudentId,ResourceParameter parameter)
        {
            if (StudentId != HttpContext.GetUserId())
                return BadRequest("You are not allow to see the Transcript!");
            var users = await _context.GetStudentTranscript(StudentId);
            var courses = await _context.GetAllStudentCourses(parameter, StudentId);

            var mapping = _mapper.Map<StudentTranscriptDto>(users);

            var cr = 0.0;
            var total = 0.0;
            var credits = 0;
            var GPA = 0.0;

            foreach (var course in courses)
            {
                if (course.LetterGrade == "A")
                {
                    cr = 4 * course.Course.CourseCredit;
                    total += cr;
                }
                if (course.LetterGrade == "B")
                {
                    cr = 3 * course.Course.CourseCredit;
                    total += cr;
                }
                if (course.LetterGrade == "C")
                {
                    cr = 2 * course.Course.CourseCredit;
                    total += cr;
                }
                if (course.LetterGrade == "D")
                {
                    cr = 1 * course.Course.CourseCredit;
                    total += cr;
                }
                if (course.LetterGrade == "F")
                {
                    cr = 0 * course.Course.CourseCredit;
                    total += cr;
                }
                credits += course.Course.CourseCredit;

            }

            mapping.GPA = total / credits;
            var result = mapping.GPA;
            double var = Math.Round(result, 2);

            users.GPA = var;
            users.Id = StudentId;
            await _userService.EditUser(users);

            return Ok("Your GPA is : " + var);
        }



        [Authorize(Roles = "Student")]
        [HttpPost("/api/Add/StudentCourse/")]
        public async Task<IActionResult> AddStudentCourse([FromBody] StudentCourseCreationDto course,ResourceParameter parameter)
        {
            if (course == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            if (course.UserId != HttpContext.GetUserId())
                return BadRequest("Not Allow to Add Course for another Teacher!");
            else
            {
                var courses = await _context.GetAllStudentCourses(parameter, course.UserId);
                var mapping = _mapper.Map<IEnumerable<StudentCourseViewDto>>(courses);

                if (mapping.Count() >= 6)
                    return BadRequest("You can not take more than 6 courses!");

                var EntityMapper = _mapper.Map<StudentCourse>(course);
                var studenttimeTables = await _timeTableService.GetStudentTimeTable(EntityMapper.UserId);
                var timeTables = await _timeTableService.GetTimeTable(EntityMapper.CourseId);

                foreach (var st in studenttimeTables)
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
                var Exist = await _context.CourseExists(EntityMapper.CourseId, course.UserId);
                if (Exist == true)
                    return BadRequest("Student Already has the course");


                var result = await _context.AddStudentCourse(EntityMapper);
                if (result == false)
                    return BadRequest("Cant add the course for user");

                return Ok(result);

            }
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("DeleteStudentCourse/{courseId}")]
        public async Task<IActionResult> DeleteStudentCourse(int courseId)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _context.GetStudentCourseById(courseId, HttpContext.GetUserId());
            if (result == null)
                return BadRequest("Course with this ID Doesn't Exist in your course list");


            await _context.RemoveStudentCourse(result);

            return Ok("Course Deleted!");
        }
        private string CreateTestListResourceUri(ResourceParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _link.GetPathByAction(HttpContext, "GetAllStudentCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber - 1,
                            PageSize = parameter.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return _link.GetPathByAction(HttpContext, "GetAllStudentCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber + 1,
                            PageSize = parameter.PageSize,
                        });
                case ResourceUriType.Current:
                    return _link.GetPathByAction(HttpContext, "GetAllStudentCourses",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });
                default:
                    return _link.GetPathByAction(HttpContext, "GetAllStudentCourses",
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








