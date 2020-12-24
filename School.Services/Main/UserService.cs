using School.DataAccess;
using School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using School.Helpers;

namespace School.Services.Main
{
    public class UserService : IUserService
    {
        private DiscDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserService(DiscDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)

        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IEnumerable<User>> GetAllDepartmentStudents( int departmentId)
        {


            var role = await _userManager.GetUsersInRoleAsync("Student");

            var v1 = await _context.Users.Include(x => x.Department).ToListAsync();


            var departmentstu = v1.Where(x => role.Any(y => y.Id == x.Id) && x.DepartmentId == departmentId).ToList();


            return departmentstu;

        }


        public async Task<IEnumerable<User>> GetAllDepartmentTeachers(int departmentId)
        {
            var role = await _userManager.GetUsersInRoleAsync("Teacher");
            var v1 = await _context.Users.Include(x => x.Department).ToListAsync();

            var departmenttea =  v1.Where(x => role.Any(y => y.Id == x.Id) && x.DepartmentId == departmentId).ToList();

            return departmenttea;
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
