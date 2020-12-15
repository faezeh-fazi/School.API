using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using School.DataTransferObject;
using School.DataTransferObject.Student;
using School.DataTransferObject.Teacher;
using School.Extensions;
using School.Helpers;
using School.Models;
using School.Services.Main;

namespace School.API.Controllers
{
    public class StudentController : ControllerBase
    {
        private IUserService _context;
        private IMapper _mapper;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private LinkGenerator _link;

        public StudentController(IUserService context, IMapper mapper, SignInManager<User> signInManager,
            UserManager<User> userManager, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _link = link;
        }

        [HttpGet("/GetAllStudents")]
        public async Task<IActionResult> GetAllStudents(ResourceParameter parameter)
        {
            var students = await _context.GetAllStudents(parameter);
            var role = await _userManager.GetUsersInRoleAsync("Student");
            var prevLink = students.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.PreviousPage) : null;
            var nextPage = students.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.NextPage) : null;
            var pageInfo = new PagingDto
            {
                totalCount = students.Count,
                pageSize = students.PageSize,
                totalPages = students.TotalPages,
                currentPages = students.CurrentPage,
                PrevLink = prevLink,
                nextLink = nextPage,
            };

            var DepartmentMapping = new StudentPaging
            {
                Students = _mapper.Map<IEnumerable<StudentViewDto>>(role),
                PagingInfo = pageInfo
            };

            return Ok(DepartmentMapping);

        }


        [HttpGet("/api/getStudent/{userId}")]
        public async Task<IActionResult> GetStudentById(string userId)
        {
            var user = await _context.GetUserById(userId);
            var mapping = _mapper.Map<StudentViewDto>(user);

            var role = await _userManager.IsInRoleAsync(user, "Student");
            if (role == true)
                return Ok(mapping);
            return BadRequest("User does not exist!");
        }


        [HttpPut("/api/UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(string userId, [FromBody] StudentUpdateDto stdUpdate)
        {

            if (stdUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _context.GetUserById(userId);

            var role = await _userManager.IsInRoleAsync(user, "Student");
            if (role == true)
            {

                user.Name = stdUpdate.StudentName;

                var result = await _context.EditUser(user);

                if (result == false)
                    return BadRequest();

                return Ok(stdUpdate);
            }
            return BadRequest("Couldnt update ");

        }

        [HttpPost("/api/StudentRegister")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddStudent([FromBody] StudentCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(creationDto);
            if (user == null)
                return BadRequest("Can not be null");

            user.DepartmentId = creationDto.DepartmentId;
            user.Name = creationDto.StudentName.ToLower();
            user.UserName = creationDto.UserName.ToLower();


            var result = await _context.AddUser(user, creationDto.Password, "Student");
            if (result == false)
                return BadRequest("failed to register");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(result);
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
