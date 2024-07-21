using AutoMapper;
using StarterAPI.Models.Entities;
using StarterAPI.Repositories;

namespace StarterAPI.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;
        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<CourseDTO> AddCourse(CourseDTO courseDTO)
        {
            var course = await _courseRepository.CreateCourse(courseDTO);
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<bool> DeleteCourse(int id)
        {
            return await _courseRepository.DeleteCourse(id);
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCourses()
        {
            var courses = await _courseRepository.GetAllCourses();
            return _mapper.Map<IEnumerable<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> GetCourseById(int id)
        {
            var course = await _courseRepository.GetCourseById(id);
            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<bool> UpdateCourse(CourseDTO courseDTO)
        {
            return await _courseRepository.UpdateCourse(courseDTO);
        }
    }
}