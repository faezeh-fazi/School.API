using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using School.DataAccess;
using School.Helpers;
using School.Models;

namespace School.Services.Main
{
    public class DepartmentService : IDepartmentService
    {
        private DiscDbContext _context;
        public DepartmentService(DiscDbContext context)
        {
            _context = context;
        }

        public async Task<PagedList<Department>> GetAllDepartments(ResourceParameter parameter)
        {
            var departments = _context
                  .Department
                  .Include(x => x.Courses)
                  .AsQueryable();


        if (!string.IsNullOrEmpty(parameter.NameFilter))
            {
                departments = departments.Where(x => x.Name.Contains(parameter.NameFilter));
            }
            return await PagedList<Department>.CreateAsync(departments, parameter.PageNumber, parameter.PageSize);

        }




        public async Task<Department> GetDepartmentById(int DepartmentId)
        {
            return await _context
                 .Department
                 .Include(x => x.Courses)
                 .FirstOrDefaultAsync(x => x.Id == DepartmentId);

        }

        public async Task<bool> AddDepartment(Department department)
        {
            await _context.Department.AddAsync(department);
            return await SaveAll();
        }
        public async Task<bool> EditDepartment(Department department)
        {
            _context.Department.Update(department);
            return await SaveAll();
        }

        public async Task<bool> RemoveDepartment(Department department)
        {
            _context.Department.Remove(department);
            return await SaveAll();
        }

        public async Task<bool> DepartmentExists(string DepartmentName)
        {
            if (await _context.Department.AnyAsync(x => x.Name == DepartmentName))
                return true;

            return false;
        }

        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
