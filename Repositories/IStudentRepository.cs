using StarterAPI.Models.Entities;

namespace StarterAPI.Repositories
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<Student> GetStudent(int id);
        Task<bool> UpdateStudent(StudentDTO studentDTO);
        Task<bool> DeleteStudent(int id);
        Task<Student> CreateStudent(StudentDTO studentDTO);
        Task<IEnumerable<Course>> GetCoursesForStudent(StudentDTO studentDTO);
    }
}