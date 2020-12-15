using System.Collections.Generic;
using System.Threading.Tasks;
using School.Models;

namespace School.Services.Main
{
    public interface IUserService
    {
        Task<bool> AddUser(User user, string password, string role);
        Task<bool> EditUser(User user);
        Task<IEnumerable<User>> GetAllStudents();
        Task<IEnumerable<User>> GetAllTeachers();
        Task<User> GetUserById(string UserId);
        Task<bool> RemoveUser(User user);
    }
}