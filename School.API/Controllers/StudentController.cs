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
    public class StudentController : ControllerBase
    {
        private IUserService _context;
        private IMapper _mapper;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        public StudentController(IUserService context, IMapper mapper, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet("/GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.GetAllStudents();
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


            var result = await _context.AddUser(user, creationDto.Password, creationDto.Role);
            if (result == false)
                return BadRequest("failed to register");

            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(result);

        }
    }
}
