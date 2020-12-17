using System.Collections.Generic;
using System.Threading.Tasks;
using School.Helpers;
using School.Models;

namespace School.Services.Main
{
    public interface IUserService
    {
        Task<bool> AddUser(User user, string password, string role);
        Task<bool> EditUser(User user);
   
        Task<IEnumerable<User>> GetAllDepartmentStudents(int departmentId);
        Task<IEnumerable<User>> GetAllDepartmentTeachers(int departmentId);
        Task<User> GetUserById(string UserId);
        Task<bool> RemoveUser(User user);
    }
}