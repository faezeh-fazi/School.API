using System.Collections.Generic;
using System.Threading.Tasks;
using School.Helpers;
using School.Models;

namespace School.Services.Main
{
    public interface IDepartmentService
    {
        Task<bool> AddDepartment(Department department);
        Task<bool> EditDepartment(Department department);
        Task<PagedList<Department>> GetAllDepartments(ResourceParameter parameter);
        Task<Department> GetDepartmentById(int DepartmentId);
        Task<bool> DepartmentExists(string DepartmentName);
        Task<bool> RemoveDepartment(Department department);
    }
}