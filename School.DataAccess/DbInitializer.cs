using School.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace School.DataAccess
{
    public class DbInitializer
    {
        public static void SeedData(DiscDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
                SeedRoles(roleManager);

            if (!context.Users.Any())
                SeedUsers(context, userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Teacher").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Teacher";
                role.NormalizedName = "TEACHER";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Student").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Student";
                role.NormalizedName = "STUDENT";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }


        }
        public static void SeedUsers(DiscDbContext context, UserManager<User> userManager)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                User admin = new User();
                admin.UserName = "Teacher";
                admin.Email = "user1@localhost";
                admin.Name = "Administrator";


                IdentityResult result = userManager.CreateAsync(admin, "Disc123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin, "Teacher").Wait();
                }
            }
        }
        public static void SeedTables(DiscDbContext context)
        {
            context.Database.EnsureCreated();

        }
    }
}
