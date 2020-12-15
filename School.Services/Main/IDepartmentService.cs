using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models;

namespace School.Services.Main
{
    public interface IDepartmentService
    {
        Task<bool> AddDepartment(Department department);
        Task<bool> EditDepartment(Department department);
        Task<IEnumerable<Department>> GetAllDepartments();
        Task<Department> GetDepartmentById(int DepartmentId);
        Task<bool> DepartmentExists(string DepartmentName);
        Task<bool> RemoveDepartment(Department department);
    }
}