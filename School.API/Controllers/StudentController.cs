using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _hostEnvironment;

        public StudentController(IUserService context, IMapper mapper, SignInManager<User> signInManager,
            UserManager<User> userManager, LinkGenerator link, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _link = link;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("/GetAllDepartmentStudents")]
        public async Task<IActionResult> GetAllStudents(int DepartmentId, ResourceParameter parameter)
        {

            var students = await _context.GetAllDepartmentStudents(DepartmentId, parameter);
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
                Students = _mapper.Map<IEnumerable<StudentViewDto>>(students),
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

        [Authorize(Roles ="Student")]
        [HttpPut("/api/UpdateStudent")]
        public async Task<IActionResult> UpdateStudent([FromBody]string userId, [FromBody] StudentUpdateDto stdUpdate)
        {

            if (stdUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            string uniqueFileName = UploadedFile(stdUpdate);

            var user = await _context.GetUserById(userId);

            var role = await _userManager.IsInRoleAsync(user, "Student");
        

                user.Name = stdUpdate.StudentName;
            user.Photo = uniqueFileName;
                var result = await _context.EditUser(user);

                if (result == false)
                    return BadRequest();

                return Ok(stdUpdate);
            

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

        private string UploadedFile(StudentUpdateDto model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
