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
        Task<PagedList<User>> GetAllStudents(ResourceParameter parameter);
        Task<PagedList<User>> GetAllTeachers(ResourceParameter parameter);

        Task<User> GetUserById(string UserId);
        Task<bool> RemoveUser(User user);
    }
}