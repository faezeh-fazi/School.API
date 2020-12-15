using School.DataAccess;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace School.Services.Main
{
    public class UserService : IUserService
    {
        private DiscDbContext _context;
        private UserManager<User> _userManager;

        public UserService(DiscDbContext context, UserManager<User> userManager)

        {
            _context = context;
            _userManager = userManager;
        }



        public async Task<IEnumerable<User>> GetAllStudents()
        {
            var res = await _userManager.GetUsersInRoleAsync("Student");

            return res;
        }


        public async Task<IEnumerable<User>> GetAllTeachers()
        {
            var res = await _userManager.GetUsersInRoleAsync("Teacher");

            return res;
        }



        public async Task<User> GetUserById(string UserId)
        {
            return await _context
                .User
                .Include(x=>x.Department)
                .FirstOrDefaultAsync(x => x.Id == UserId);
        }

        public async Task<bool> AddUser(User user, string password, string role)
        {
            var existingUser = await _userManager.FindByNameAsync(user.UserName);

            if (existingUser != null)
            {
                return false;
            }

            user.IsActive = true;

            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            else
            {
                return false;
            }

        }
        public async Task<bool> EditUser(User user)
        {
            _context.User.Update(user);
            return await SaveAll();
        }

       

        public async Task<bool> RemoveUser(User user)
        {
            _context.User.Remove(user);
            return await SaveAll();
        }

        private async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
