using System.ComponentModel.DataAnnotations;
using TrainingManagementApi.Models;

namespace TrainingManagementApi.DTOs
{
    public class UpdateTrainingStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        [AllowedValues(TrainingStatuses.Assigned, TrainingStatuses.InProgress, TrainingStatuses.Completed,
        ErrorMessage = "Status must be one of: Assigned, In Progress, Completed.")]
        public string Status { get; set; } = string.Empty;
    }
}
