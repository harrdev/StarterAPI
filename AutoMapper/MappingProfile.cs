using AutoMapper;
using StarterAPI.Models.Entities;

namespace StarterAPI.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.Courses.Select(c => c.CourseId).ToList()));

            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.StudentIds, opt => opt.MapFrom(src => src.Students.Select(s => s.Id).ToList()));

            CreateMap<StudentDTO, Student>()
                .ForMember(dest => dest.Courses, opt => opt.Ignore());

            CreateMap<CourseDTO, Course>()
                .ForMember(dest => dest.Students, opt => opt.Ignore());
        }
    }
}