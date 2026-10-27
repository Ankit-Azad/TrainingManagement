using System.ComponentModel.DataAnnotations;

namespace TrainingManagementApi.DTOs
{
    public class EmployeeRequestDto
    {
        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100, ErrorMessage = "Employee name cannot exceed 100 characters.")]
        public string EmployeeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is not a valid email address.")]
        [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Joining date is required.")]
        public DateTime? JoiningDate { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Experience cannot be negative.")]
        public int Experience { get; set; }
    }
}
