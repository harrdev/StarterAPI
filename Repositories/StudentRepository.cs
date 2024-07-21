using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StarterAPI.Models.Entities;

namespace StarterAPI.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public StudentRepository(DataContext context, IMapper mapper)
        { 
            _context = context;
            _mapper = mapper;
        }
        public async Task<Student> CreateStudent(StudentDTO studentDTO)
        {
            var student = _mapper.Map<Student>(studentDTO);
            var courses = await _context.Courses.Where(c => studentDTO.CourseIds.Contains(c.CourseId)).ToListAsync();
            student.Courses = courses;
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Student> GetStudent(int id)
        {
            return await _context.Students
            .Include(s => s.Courses) // Get related courses
            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.Students
                .Include(s => s.Courses)
                .ToListAsync();
        }

        public async Task<bool> UpdateStudent(StudentDTO studentDTO)
        {
            var student = await _context.Students
                .Include(s => s.Courses)
                .FirstOrDefaultAsync(s => s.Id == studentDTO.Id);

            if (student == null) return false;

            _mapper.Map(studentDTO, student);

            student.Courses.Clear();

            // Add the courses from the DTO
            foreach (var courseId in studentDTO.CourseIds)
            {
                var course = await _context.Courses.FindAsync(courseId);
                if (course != null)
                {
                    student.Courses.Add(course);
                }
            }
            return await _context.SaveChangesAsync() > 0;
        }
    }
}