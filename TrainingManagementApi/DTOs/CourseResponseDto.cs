namespace TrainingManagementApi.DTOs
{
    public class CourseResponseDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public string TrainerName { get; set; } = string.Empty;
    }
}
