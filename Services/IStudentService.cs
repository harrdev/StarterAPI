using StarterAPI.Models.Entities;

namespace StarterAPI.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDTO>> GetAllStudents();
        Task<StudentDTO> GetStudentById(int id);
        Task<bool> UpdateStudent(StudentDTO studentDTO);
        Task<bool> DeleteStudent(int id);
        Task<StudentDTO> AddStudent(StudentDTO studentDTO);
        Task<IEnumerable<CourseDTO>> GetCoursesForStudent(int id);
    }
}