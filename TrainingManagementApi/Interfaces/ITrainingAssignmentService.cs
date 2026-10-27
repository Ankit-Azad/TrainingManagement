using TrainingManagementApi.DTOs;


namespace TrainingManagementApi.Interfaces
{
    public interface ITrainingAssignmentService
    {
        Task<TrainingAssignmentResponseDto> AssignAsync(AssignTrainingDto dto);
        Task<TrainingAssignmentResponseDto> GetByIdAsync(int id);
        Task<List<TrainingAssignmentResponseDto>> GetByEmployeeAsync(int employeeId);
        Task<TrainingAssignmentResponseDto> UpdateStatusAsync(int id, UpdateTrainingStatusDto dto);
        Task DeleteAsync(int id);


    }
}
