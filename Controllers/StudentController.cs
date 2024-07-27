using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StarterAPI.Attributes;
using StarterAPI.Models.Entities;
using StarterAPI.Services;

namespace StarterAPI.Controllers
{
    [ApiController]
    [ApiKey]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public StudentController
            (IStudentService studentService,
            ILogger<StudentController> logger,
            IMapper mapper,
            DataContext context)
        {
            _studentService = studentService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}/courses")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCoursesByStudent(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching courses for student {id}.");
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                {
                    _logger.LogInformation($"Student {id} not found.");
                    return NotFound();
                }
                var courses = await _studentService.GetCoursesForStudent(id);
                return Ok(courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting courses for student {id}.");
                return StatusCode(500, "An error occurred getting courses for that student.");
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 30)] // Cache response for 30 seconds
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            try
            {
                _logger.LogInformation("Fetching all students");
                var students = await _studentService.GetAllStudents();
                var studentDTOs = _mapper.Map<IEnumerable<StudentDTO>>(students);
                return Ok(studentDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all students");
                return StatusCode(500, "An error occurred while fetching all students.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent(StudentDTO studentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating student");
                var createdStudentDTO = await _studentService.AddStudent(studentDTO);
                if (createdStudentDTO == null)
                {
                    _logger.LogInformation("Student creation failed");
                    return BadRequest();
                }

                return CreatedAtAction(nameof(GetStudent), new { id = createdStudentDTO.Id }, createdStudentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error creating student");
                return StatusCode(500, "There was an error creating a student.");
            }
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "id" })]
        public async Task<ActionResult<StudentDTO>> GetStudent(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching student id: {id}");
                var student = await _studentService.GetStudentById(id);
                if (student == null)
                {
                    _logger.LogInformation($"Student {id} requested and does not exist");
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching student {id}");
                return StatusCode(500, $"An error occurred fetching student {id}.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting student {id}");
                var success = await _studentService.DeleteStudent(id);
                if (!success)
                {
                    _logger.LogInformation($"Student {id} not deleted, does not exist.");
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting student {id}.");
                return StatusCode(500, "There was an error deleting student.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, StudentDTO updateStudent)
        {
            if (id != updateStudent.Id)
            {
                _logger.LogInformation($"ID in the URL does not match ID in the request body.");
                return BadRequest("ID in the URL does not match ID in the request body.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation($"Updating student {id}.");
                var success = await _studentService.UpdateStudent(updateStudent);
                if (!success)
                {
                    _logger.LogInformation($"No student found with id {id}.");
                    return NotFound(new { Message = $"No student found with ID {id}." });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating student {id}");
                return StatusCode(500, "There was an error updating the student.");
            }
        }
    }
}