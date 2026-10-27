using TrainingManagementApi.DTOs;

namespace TrainingManagementApi.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllAsync();
        Task<EmployeeResponseDto> GetByIdAsync(int id);
        Task<EmployeeResponseDto> CreateAsync(EmployeeRequestDto dto);
        Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeRequestDto dto);
        Task DeleteAsync(int id);
    }
}
