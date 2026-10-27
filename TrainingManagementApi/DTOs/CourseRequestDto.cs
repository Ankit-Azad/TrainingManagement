using System.ComponentModel.DataAnnotations;


namespace TrainingManagementApi.DTOs
{
    public class CourseRequestDto
    {
        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(150, ErrorMessage = "Course name cannot exceed 150 characters.")]
        public string CourseName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Course duration must be greater than zero.")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Trainer name is required.")]
        [StringLength(100, ErrorMessage = "Trainer name cannot exceed 100 characters.")]
        public string TrainerName { get; set; } = string.Empty;


    }
}
