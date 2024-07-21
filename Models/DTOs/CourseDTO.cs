using System.ComponentModel.DataAnnotations;

public class CourseDTO
{
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Course name must not be empty.")]
    public string Name { get; set; }
    public List<int> StudentIds { get; set; }
}