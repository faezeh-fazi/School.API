using System;
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

        [HttpGet("/GetAllDepartmentStudents")]
        public async Task<IActionResult> GetAllStudents(int DepartmentId)
        {

                var students = await _context.GetAllDepartmentStudents(DepartmentId);
                var mapping = _mapper.Map<IEnumerable<StudentViewDto>>(students);

                return Ok(mapping);

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

        [Authorize(Roles ="Student")]
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
        [Authorize(Roles ="Admin")]
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
                    return _link.GetPathByAction(HttpContext, "GetAllStudents",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber - 1,
                            PageSize = parameter.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return _link.GetPathByAction(HttpContext, "GetAllStudents",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber + 1,
                            PageSize = parameter.PageSize,
                        });
                case ResourceUriType.Current:
                    return _link.GetPathByAction(HttpContext, "GetAllStudents",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });
                default:
                    return _link.GetPathByAction(HttpContext, "GetAllStudents",
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
