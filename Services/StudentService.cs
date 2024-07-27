using AutoMapper;
using StarterAPI.Models.Entities;
using StarterAPI.Repositories;

namespace StarterAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesForStudent(int id)
        {
            var student = await _studentRepository.GetStudent(id);
            var studentDTO = _mapper.Map<StudentDTO>(student);
            var courses = await _studentRepository.GetCoursesForStudent(studentDTO);
            var courseDTOs = _mapper.Map<IEnumerable<CourseDTO>>(courses);
            return courseDTOs;
        }

        public async Task<StudentDTO> AddStudent(StudentDTO studentDTO)
        {
            var student = await _studentRepository.CreateStudent(studentDTO);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<bool> DeleteStudent(int id)
        {
            return await _studentRepository.DeleteStudent(id);
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudents()
        {
            var students = await _studentRepository.GetStudents();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetStudentById(int id)
        {
            var student = await _studentRepository.GetStudent(id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<bool> UpdateStudent(StudentDTO studentDTO)
        {
            return await _studentRepository.UpdateStudent(studentDTO);
        }
    }
}