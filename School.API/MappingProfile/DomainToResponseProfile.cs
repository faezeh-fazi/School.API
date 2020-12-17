using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using School.DataTransferObject;
using School.DataTransferObject.Course;
using School.DataTransferObject.Department;
using School.DataTransferObject.Student;
using School.DataTransferObject.StudentCourse;
using School.DataTransferObject.StudentGrade;
using School.DataTransferObject.StudentTimeTable;
using School.DataTransferObject.Teacher;
using School.DataTransferObject.TeacherCourse;
using School.DataTransferObject.TeacherTimeTable;
using School.DataTransferObject.TimeTable;
using School.Models;

namespace School.API.MappingProfile
{
    public class DomainToResponseProfile:Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<TeacherCreationDto, TeacherCourse>();
            CreateMap<TeacherUpdateDto, TeacherCourse>();
            CreateMap<TeacherCourse, TeacherViewDto>()
                .ForMember(dest => dest.TeacherName, m => m.MapFrom(src => src.User.Name));

            CreateMap<StudentCreationDto, StudentCourse>();
            CreateMap<StudentUpdateDto, StudentCourse>();
            CreateMap<StudentCourse, StudentViewDto>()
                .ForMember(dest => dest.StudentName, m => m.MapFrom(src => src.User.Name));

            CreateMap<StudentCreationDto, User>();
            CreateMap<StudentUpdateDto, User>();
            CreateMap<User, StudentViewDto>()
                .ForMember(dest => dest.StudentName, m => m.MapFrom(src => src.Name))
                 .ForMember(dest => dest.DepartmentName, m => m.MapFrom(src => src.Department.Name));




            CreateMap<TeacherCreationDto, User>();
            CreateMap<TeacherUpdateDto, User>();
            CreateMap<User, TeacherViewDto>()
                .ForMember(dest => dest.TeacherName, m => m.MapFrom(src => src.Name))
                .ForMember(dest => dest.DepartmentName, m => m.MapFrom(src => src.Department.Name));




            CreateMap<TeacherDepartmentDto, User>();


            CreateMap<CourseCreationDto, Course>();       
            CreateMap<CourseUpdateDto, Course>();
            CreateMap<Course, CourseViewDto>()
                .ForMember(dest => dest.StartDate, m => m.MapFrom(src => src.StartDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.EndDate, m => m.MapFrom(src => src.EndDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.Department, m => m.MapFrom(src => src.Department.Name));




            CreateMap<TeacherCreationDto, Course>();
            CreateMap<TeacherUpdateDto, Course>();
            CreateMap<Course, TeacherViewDto>();


            CreateMap<TeacherCourseCreationDto, TeacherCourse>();
            CreateMap<TeacherCourseUpdateDto, TeacherCourse>();
            CreateMap<TeacherCourse, TeacherCourseViewDto>()
             .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
             .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription));


            CreateMap<StudentCourseCreationDto, StudentCourse>();
            CreateMap<StudentCourseUpdateDto, StudentCourse>();
            CreateMap<StudentCourse, StudentCourseViewDto>()
                  .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                  .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription));
               


            CreateMap<TeacherCourseCreationDto, Course>();
            CreateMap<TeacherCourseUpdateDto, Course>();
            CreateMap<Course, TeacherCourseViewDto>();

            CreateMap<StudentCourseCreationDto, Course>();
            CreateMap<StudentCourseUpdateDto, Course>();
            CreateMap<Course, StudentCourseViewDto>();
                
            CreateMap<CourseCreationDto, TimeTable>();
            CreateMap<CourseUpdateDto, TimeTable>();
            CreateMap<TimeTable, CourseViewDto>()
                 .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
             .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                .ForMember(dest => dest.StartDate, m => m.MapFrom(src => src.Course.StartDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.EndDate, m => m.MapFrom(src => src.Course.EndDate.ToString("MM/dd/yyyy")));
            


            CreateMap<TimeTableCreationDto, TimeTable>();
            CreateMap<TimeTableUpdateDto, TimeTable>();
            CreateMap<TimeTable, TimeTableViewDto>()
                 .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                 .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Day.ToString()));



            CreateMap<TimeTableCreationDto, Course>();
            CreateMap<TimeTableUpdateDto, Course>();
            CreateMap<Course, TimeTableViewDto>()
                .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.CourseDescription))
                .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Day.ToString()));


            CreateMap<StudentTimeTableCreationDto, TimeTable>();
            CreateMap<StudentTimeTableUpdateDto, TimeTable>();
            CreateMap<TimeTable, StudentTimetableViewDto>()
                 .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                 .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Day.ToString()));



            CreateMap<StudentTimeTableCreationDto, StudentCourse>();
            CreateMap<StudentTimeTableUpdateDto, StudentCourse>();
            CreateMap<StudentCourse, StudentTimetableViewDto>()
                .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.Course.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.Course.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Course.Day.ToString()));


            CreateMap<TeacherTimeTableCreationDto, TimeTable>();
            CreateMap<TeacherTimeTableUpdateDto, TimeTable>();
            CreateMap<TimeTable, TeacherTimetableViewDto>()
                 .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                 .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Day.ToString()));



            CreateMap<TeacherTimeTableCreationDto, TeacherCourse>();
            CreateMap<TeacherTimeTableUpdateDto, TeacherCourse>();
            CreateMap<TeacherCourse, TeacherTimetableViewDto>()
                .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName))
                 .ForMember(dest => dest.CourseDescription, m => m.MapFrom(src => src.Course.CourseDescription))
                .ForMember(dest => dest.StartTime, m => m.MapFrom(src => src.Course.StartTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.EndTime, m => m.MapFrom(src => src.Course.EndTime.ToString("hh:mm tt")))
                .ForMember(dest => dest.Day, m => m.MapFrom(src => src.Course.Day.ToString()));



            CreateMap<DepartmentUpdateDto, Department>();
            CreateMap<DepartmentCreationDto, Department>();
            CreateMap<Department, DepartmentViewDto>();



            CreateMap<DepartmentUpdateDto, Course>();
            CreateMap<DepartmentCreationDto, Course>();
            CreateMap<Course, DepartmentViewDto>()
                .ForMember(dest => dest.Name, m => m.MapFrom(src => src.Department.Name));



            CreateMap<StudentGradeCreationDto, StudentCourse>();
            CreateMap<StudentGradeUpdateDto, StudentCourse>();
            CreateMap<StudentCourse, StudentGradeViewDto>()
                .ForMember(dest => dest.CourseName, m => m.MapFrom(src => src.Course.CourseName));


            CreateMap<StudentCourse, StudentTranscriptDto>();
            CreateMap<User, StudentTranscriptDto>().ReverseMap()
                .ForMember(dest => dest.GPA, m => m.MapFrom(src => src.GPA.ToString("F2")));




            CreateMap<AdminCreationDto, User>();
            
        }
    }
}

 