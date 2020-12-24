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
    public class TeacherController : Controller
    {

        private IUserService _context;
        private IMapper _mapper;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private LinkGenerator _link;

        public TeacherController(IUserService context, IMapper mapper,
            SignInManager<User> signInManager, UserManager<User> userManager, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _link = link;

        }

        [HttpGet("/GetAllDepartmentTeachers")]
        public async Task<IActionResult> GetAllTeachers(int DepartmentId)
        {

            var teachers = await _context.GetAllDepartmentTeachers(DepartmentId);
            var mapping = _mapper.Map<IEnumerable<TeacherViewDto>>(teachers);

            return Ok(mapping);
        }

        [HttpGet("/api/getTeacher/{userId}")]
        public async Task<IActionResult> GetTeacherById(string userId)
        {
            var user = await _context.GetUserById(userId);

            var mapping = _mapper.Map<TeacherViewDto>(user);

            var role = await _userManager.IsInRoleAsync(user, "Teacher");
            if (role == true)
                return Ok(mapping);
            return BadRequest("User does not exist!");
        }
        [Authorize(Roles="Teacher")]
        [HttpPut("/api/UpdateTeacher")]
        public async Task<IActionResult> UpdateTeacher(string userId, [FromBody] TeacherUpdateDto tchUpdate)
        {

            if (tchUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();


            var user = await _context.GetUserById(userId);

            var role = await _userManager.IsInRoleAsync(user, "Teacher");
            if (role == true)
            {

                user.Name = tchUpdate.TeacherName;

                var result = await _context.EditUser(user);

                if (result == false)
                    return BadRequest();

                return Ok(tchUpdate);
            }
            return BadRequest("Couldnt update");

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/TeacherRegister")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            var user = _mapper.Map<User>(creationDto);

            user.Name = creationDto.TeacherName.ToLower();
            user.UserName = creationDto.UserName.ToLower();


            var result = await _context.AddUser(user, creationDto.Password, "Teacher");
            if (result == false)
                return BadRequest("failed to register");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(result);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/AdminRegister")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddAdmin([FromBody] AdminCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(creationDto);

            user.Name = creationDto.AdminName.ToLower();
            user.UserName = creationDto.UserName.ToLower();

            var result = await _context.AddUser(user, creationDto.Password, "Admin");
            if (result == false)
                return BadRequest("failed to register");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(result);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/api/AddDepartment")]
        public async Task<IActionResult> AddTeacherDepartment(string TeacherId, [FromBody] TeacherDepartmentDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

         
                if (creationDto.DepartmentId == null)
                    return BadRequest("Add Department");

                var Entity = await _context.GetUserById(TeacherId);

                Entity.DepartmentId = creationDto.DepartmentId;

                var result = await _context.EditUser(Entity);
                return Ok(result);

            
        }
            private string CreateTestListResourceUri(ResourceParameter parameter, ResourceUriType type)
            {
                switch (type)
                {
                    case ResourceUriType.PreviousPage:
                        return _link.GetPathByAction(HttpContext, "GetAllTeachers",
                            values: new
                            {
                                searcQuery = parameter.SearchQuery,
                                pageNumber = parameter.PageNumber - 1,
                                PageSize = parameter.PageSize,
                            });

                    case ResourceUriType.NextPage:
                        return _link.GetPathByAction(HttpContext, "GetAllTeachers",
                            values: new
                            {
                                searcQuery = parameter.SearchQuery,
                                pageNumber = parameter.PageNumber + 1,
                                PageSize = parameter.PageSize,
                            });
                    case ResourceUriType.Current:
                        return _link.GetPathByAction(HttpContext, "GetAllTeachers",
                            values: new
                            {
                                searcQuery = parameter.SearchQuery,
                                pageNumber = parameter.PageNumber,
                                PageSize = parameter.PageSize,
                            });
                    default:
                        return _link.GetPathByAction(HttpContext, "GetAllTeachers",
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
