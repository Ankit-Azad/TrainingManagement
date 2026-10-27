namespace TrainingManagementApi.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Duration { get; set; }          // in hours
        public string TrainerName { get; set; } = string.Empty;

        // One Course -> Many TrainingAssignments
        public ICollection<TrainingAssignment> TrainingAssignments { get; set; } = new List<TrainingAssignment>();
    }
}
