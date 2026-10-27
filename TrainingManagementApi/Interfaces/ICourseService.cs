using TrainingManagementApi.DTOs;


namespace TrainingManagementApi.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponseDto>> GetAllAsync();
        Task<CourseResponseDto> GetByIdAsync(int id);
        Task<CourseResponseDto> CreateAsync(CourseRequestDto dto);
        Task<CourseResponseDto> UpdateAsync(int id, CourseRequestDto dto);
        Task DeleteAsync(int id);

    }
}
