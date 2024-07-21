using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StarterAPI.Models.Entities;

namespace StarterAPI.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CourseRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Course> CreateCourse(CourseDTO courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);
            var students = await _context.Students.Where(s => courseDTO.StudentIds.Contains(s.Id)).ToListAsync();
            course.Students = students;
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return false;

            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses
                .Include(s => s.Students)
                .ToListAsync();
        }

        public async Task<Course> GetCourseById(int id)
        {
            return await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.CourseId == id);
        }

        public async Task<bool> UpdateCourse(CourseDTO courseDTO)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(s => s.CourseId == courseDTO.CourseId);

            if (course == null) return false;

            _mapper.Map(courseDTO, course);

            course.Students.Clear();

            // Add students from the DTO
            foreach (var id in courseDTO.StudentIds)
            {
                var student = await _context.Students.FindAsync(id);
                if (student != null)
                {
                    course.Students.Add(student);
                }
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}