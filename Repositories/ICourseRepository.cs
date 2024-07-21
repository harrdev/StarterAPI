using StarterAPI.Models.Entities;

namespace StarterAPI.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course> GetCourseById(int id);
        Task<bool> DeleteCourse(int id);
        Task<bool> UpdateCourse(CourseDTO courseDTO);
        Task<Course> CreateCourse(CourseDTO courseDTO);
    }
}