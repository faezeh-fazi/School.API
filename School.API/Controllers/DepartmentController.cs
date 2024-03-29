﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using School.DataTransferObject;
using School.DataTransferObject.Department;
using School.Helpers;
using School.Models;
using School.Services.Main;

namespace School.API.Controllers
{
   
    public class DepartmentController : ControllerBase
    {
        private IDepartmentService _context;
        private IMapper _mapper;
        private LinkGenerator _link;

        public DepartmentController(IDepartmentService context, IMapper mapper, LinkGenerator link)
        {
            _context = context;
            _mapper = mapper;
            _link = link;
        }
        [HttpGet("/GetAlldepartments")]

        public async Task<IActionResult> GetAllDepartments(ResourceParameter parameter)
        {
            var departments = await _context.GetAllDepartments(parameter);
            var prevLink = departments.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.PreviousPage) : null;
            var nextPage = departments.HasPrevious ? CreateTestListResourceUri(parameter, ResourceUriType.NextPage) : null;
            var pageInfo = new PagingDto
            {
                totalCount = departments.Count,
                pageSize = departments.PageSize,
                totalPages = departments.TotalPages,
                currentPages = departments.CurrentPage,
                PrevLink = prevLink,
                nextLink = nextPage,
            };
            var DepartmentMapping = new DepartmentPaging
            {
                Departments = _mapper.Map<IEnumerable<DepartmentViewDto>>(departments),
                PagingInfo = pageInfo
            };

            return Ok(DepartmentMapping);
        }



        [HttpGet("/api/Getdepartment")]
        public async Task<IActionResult> GetDepartmentById(int DepId)
        {
            var department = await _context.GetDepartmentById(DepId);
            if (department == null)
                return BadRequest("Department doesnt exist!");
            var mapping = _mapper.Map<DepartmentViewDto>(department);
            return Ok(mapping);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("/AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentCreationDto department)
        {
            if (department == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();


            var EntityMapper = _mapper.Map<Department>(department);
            
            var Exist = await _context.DepartmentExists(EntityMapper.Name);
            if (Exist== true)
                return BadRequest("Department with this name already Exist!!");

            var result = await _context.AddDepartment(EntityMapper);

            if (result == false)
                return BadRequest("Enter the Department");

            return Ok(result);

        }


        [Authorize(Roles = "Admin")]
        [HttpPut("updatedepartment/{departmentId}")]
        public async Task<IActionResult> UpdateDepartment(int departmentId, [FromBody] DepartmentUpdateDto department)
        {

            if (department == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            var departmentFromDb = await _context.GetDepartmentById(departmentId);
            if (departmentFromDb == null)
                return BadRequest("Department Doesnt exist!");


            departmentFromDb.Name = department.Name;
         
            var result = await _context.EditDepartment(departmentFromDb);

            if (result == false)
                return BadRequest();

            return Ok(department);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("deletedepartment/{departmentId}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _context.GetDepartmentById(departmentId);
            if (result == null)
                return BadRequest("Department doesnt exist!");

            await _context.RemoveDepartment(result);

            return Ok("department Deleted");
        }

        private string CreateTestListResourceUri(ResourceParameter parameter, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _link.GetPathByAction(HttpContext, "GetAllDepartments",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber - 1,
                            PageSize = parameter.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return _link.GetPathByAction(HttpContext, "GetAllDepartments",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber + 1,
                            PageSize = parameter.PageSize,
                        });
                case ResourceUriType.Current:
                    return _link.GetPathByAction(HttpContext, "GetAllDepartments",
                        values: new
                        {
                            searcQuery = parameter.SearchQuery,
                            pageNumber = parameter.PageNumber,
                            PageSize = parameter.PageSize,
                        });
                default:
                    return _link.GetPathByAction(HttpContext, "GetAllDepartments",
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
