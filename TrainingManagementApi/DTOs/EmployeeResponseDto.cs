namespace TrainingManagementApi.DTOs
{
    public class EmployeeResponseDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public int Experience { get; set; }
    }
}
