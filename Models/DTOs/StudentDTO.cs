using System.ComponentModel.DataAnnotations;

public class StudentDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 character long.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 character long.")]
    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
    public List<int> CourseIds { get; set; }
}