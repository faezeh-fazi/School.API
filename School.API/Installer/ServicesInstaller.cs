using School.DataAccess;
using School.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using DiscSyria.API.Installer;
using Microsoft.Extensions.DependencyInjection;
using School.Contracts;
using School.LoggingService;
using School.Services.Main;

namespace School.API.Installer
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServicesAssembly(IServiceCollection services, IConfiguration configuration)
        {
            var ConnectionString = configuration["connectionString:DiscDbConnection"];
            services.AddDbContext<DiscDbContext>(item =>
            item.UseSqlServer(ConnectionString, options => options.CommandTimeout(180)))
                .AddDefaultIdentity<User>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DiscDbContext>();

            services.AddCors(options => options.AddPolicy("DiscPolicy",
                builder =>
                {
                    //for signalR you must use .SetIsOriginAllowed((host)=>true) with .AllowCredentials()
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }));


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentCourseService, StudentCourseService>();
            services.AddScoped<ITeacherCourseService, TeacherCourseService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ITimeTableService, TimeTableService>();
            services.AddScoped<IDepartmentService, DepartmentService>();




            services.AddScoped<ILoggerManager, LoggerManager>();

            services.AddControllers();
        }
    }
}
