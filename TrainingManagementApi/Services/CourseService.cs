using Microsoft.EntityFrameworkCore;
using TrainingManagementApi.Data;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Exceptions;
using TrainingManagementApi.Interfaces;
using TrainingManagementApi.Models;

namespace TrainingManagementApi.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CourseService> _logger;

        public CourseService(AppDbContext context, ILogger<CourseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<CourseResponseDto>> GetAllAsync()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.CourseId)
                .ToListAsync();

            return courses.Select(ToDto).ToList();
        }

        public async Task<CourseResponseDto> GetByIdAsync(int id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == id)
                ?? throw new NotFoundException($"Course with id {id} was not found.");

            return ToDto(course);
        }

        public async Task<CourseResponseDto> CreateAsync(CourseRequestDto dto)
        {
            var course = new Course
            {
                CourseName = dto.CourseName.Trim(),
                Duration = dto.Duration,
                TrainerName = dto.TrainerName.Trim()
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course {CourseId} created", course.CourseId);
            return ToDto(course);
        }

        public async Task<CourseResponseDto> UpdateAsync(int id, CourseRequestDto dto)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id)
                ?? throw new NotFoundException($"Course with id {id} was not found.");

            course.CourseName = dto.CourseName.Trim();
            course.Duration = dto.Duration;
            course.TrainerName = dto.TrainerName.Trim();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Course {CourseId} updated", id);
            return ToDto(course);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id)
                ?? throw new NotFoundException($"Course with id {id} was not found.");

            var assignmentCount = await _context.TrainingAssignments.CountAsync(a => a.CourseId == id);
            if (assignmentCount > 0)
                throw new BusinessValidationException(
                    $"Course '{course.CourseName}' cannot be deleted because it is assigned in {assignmentCount} training assignment(s).");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course {CourseId} deleted", id);
        }

        private static CourseResponseDto ToDto(Course c) => new()
        {
            CourseId = c.CourseId,
            CourseName = c.CourseName,
            Duration = c.Duration,
            TrainerName = c.TrainerName
        };


    }
}
