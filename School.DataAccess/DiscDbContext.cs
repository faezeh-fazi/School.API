using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.DataAccess
{
    public class DiscDbContext:IdentityDbContext<User>
    {
        public DiscDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<TeacherCourse> TeacherCourse { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }
        public DbSet<TimeTable> TimeTable { get; set; }

        public DbSet<Department> Department { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }





        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<StudentCourse>().HasKey(p=> new { p.UserId, p.CourseId });
        //    modelBuilder.Entity<StudentCourse>().HasOne(p => p.Course).WithMany(p => p.StudentCourses).HasForeignKey(p => p.CourseId);
        //    modelBuilder.Entity<StudentCourse>().HasOne(p => p.User).WithMany(p => p.StudentCourses).HasForeignKey(p => p.UserId);

        //    modelBuilder.Entity<TeacherCourse>().HasKey(p => new { p.UserId, p.CourseId });
        //    modelBuilder.Entity<TeacherCourse>().HasOne(p => p.Course).WithMany(p => p.TeacherCourses).HasForeignKey(p => p.CourseId);
        //    modelBuilder.Entity<TeacherCourse>().HasOne(p => p.User).WithMany(p => p.TeacherCourses).HasForeignKey(p => p.UserId);
        //}
    }
}
