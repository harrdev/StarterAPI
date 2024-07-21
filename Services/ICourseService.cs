using StarterAPI.Models.Entities;

namespace StarterAPI.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCourses();
        Task<CourseDTO> GetCourseById(int id);
        Task<bool> UpdateCourse(CourseDTO courseDTO);
        Task<bool> DeleteCourse(int id);
        Task<CourseDTO> AddCourse(CourseDTO courseDTO);
    }
}