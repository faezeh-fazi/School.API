using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.DataTransferObject;
using School.DataTransferObject.Student;
using School.DataTransferObject.Teacher;
using School.Extensions;
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

        public TeacherController(IUserService context, IMapper mapper,
            SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;


        }

        [HttpGet("/GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var teachers = await _context.GetAllTeachers();
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
        [HttpPost("/api/TeacherRegister")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherCreationDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            var user = _mapper.Map<User>(creationDto);

            user.Name = creationDto.TeacherName.ToLower();
            user.UserName = creationDto.UserName.ToLower();


            var result = await _context.AddUser(user, creationDto.Password, creationDto.Role);
            if (result == false)
                return BadRequest("failed to register");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(result);

        }
        [Authorize(Roles ="Teacher")]
        [HttpPost("/api/TeacherDepartment")]
        public async Task<IActionResult> AddTeacherDepartment(string TeacherId, [FromBody] TeacherDepartmentDto creationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (TeacherId != HttpContext.GetUserId())
                return BadRequest("Not Allow to Add Department!");
            else
            {
                if (creationDto.DepartmentId == null)
                    return BadRequest("Add Department");

                var Entity = await _context.GetUserById(TeacherId);

                Entity.DepartmentId = creationDto.DepartmentId;

                var result = await _context.EditUser(Entity);
                return Ok(result);

            }
        }

    }
}
