using System.ComponentModel.DataAnnotations;


namespace TrainingManagementApi.DTOs
{
    public class AssignTrainingDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "A valid EmployeeId is required.")]
        public int EmployeeId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid CourseId is required.")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime? EndDate { get; set; }

    }
}
