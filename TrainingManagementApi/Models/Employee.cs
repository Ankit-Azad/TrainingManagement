namespace TrainingManagementApi.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
        public int Experience { get; set; }

        // One Employee -> Many TrainingAssignments
        public ICollection<TrainingAssignment> TrainingAssignments { get; set; } = new List<TrainingAssignment>();

    }
}
