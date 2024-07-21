using System.ComponentModel.DataAnnotations;

namespace StarterAPI.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property
        public ICollection<Student> Students { get; set; }
    }
}
