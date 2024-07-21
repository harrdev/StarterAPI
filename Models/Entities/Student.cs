using System.ComponentModel.DataAnnotations;

namespace StarterAPI.Models.Entities
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }

        // Navigation property
        public ICollection<Course> Courses { get; set; }
    }
}