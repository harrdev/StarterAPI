using Microsoft.AspNetCore.Mvc;
using StarterAPI.Attributes;
using StarterAPI.Services;

namespace StarterAPI.Controllers
{
    [ApiController]
    [ApiKey]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CourseController> _logger;
        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            try
            {
                _logger.LogInformation("Fetching all courses.");
                var courseDTOs = await _courseService.GetAllCourses();
                return Ok(courseDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all courses.");
                return StatusCode(500, "There was an error fetching all courses.");
            }
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "id" })]
        public async Task<ActionResult<CourseDTO>> GetCourse(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching course {id}.");
                var courseDTO = await _courseService.GetCourseById(id);
                if (courseDTO == null)
                {
                    _logger.LogInformation($"Course {id} request and does not exist.");
                    return NotFound(new { Message = $"No course found with ID {id}." });
                }
                return Ok(courseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching course {id}.");
                return StatusCode(500, $"An error occurred fetching course {id}.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseDTO courseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating Course.");
                var createdCourseDTO = await _courseService.AddCourse(courseDTO);
                if (createdCourseDTO == null)
                {
                    _logger.LogInformation("Course creation failed.");
                    return BadRequest();
                }

                return CreatedAtAction(nameof(GetCourse), new { id = createdCourseDTO.CourseId }, createdCourseDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course.");
                return StatusCode(500, "There was an error creating course.");
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting course {id}.");
                var success = await _courseService.DeleteCourse(id);

                if (!success)
                {
                    _logger.LogInformation($"Could not delete course {id}, course does not exist.");
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting course {id}.");
                return StatusCode(500, "There was an error deleting course");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, CourseDTO courseDTO)
        {

            if (id != courseDTO.CourseId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(courseDTO);
            }

            try
            {
                _logger.LogInformation($"Updating course {id}.");
                var success = await _courseService.UpdateCourse(courseDTO);
                if (!success)
                {
                    _logger.LogInformation($"No course found with id {id}.");
                    return NotFound(new { Message = $"No course found with ID {id}." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating course {id}.");
                return StatusCode(505, "Error updating course.");
            }
        }
    }
}