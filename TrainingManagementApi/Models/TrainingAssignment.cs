namespace TrainingManagementApi.Models
{
    public class TrainingAssignment
    {
        public int AssignmentId { get; set; }

        public int EmployeeId { get; set; }                       // foreign key
        public Employee Employee { get; set; } = null!;           // navigation

        public int CourseId { get; set; }                         // foreign key
        public Course Course { get; set; } = null!;               // navigation

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = TrainingStatuses.Assigned;
    }
}
