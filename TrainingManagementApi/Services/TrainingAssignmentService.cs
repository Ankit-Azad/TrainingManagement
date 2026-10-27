using Microsoft.EntityFrameworkCore;
using TrainingManagementApi.Data;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Exceptions;
using TrainingManagementApi.Interfaces;
using TrainingManagementApi.Models;


namespace TrainingManagementApi.Services
{
    public class TrainingAssignmentService : ITrainingAssignmentService
    {

        private readonly AppDbContext _context;
        private readonly ILogger<TrainingAssignmentService> _logger;

        public TrainingAssignmentService(AppDbContext context, ILogger<TrainingAssignmentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TrainingAssignmentResponseDto> AssignAsync(AssignTrainingDto dto)
        {
            var startDate = dto.StartDate!.Value;
            var endDate = dto.EndDate!.Value;

            if (endDate < startDate)
                throw new BusinessValidationException("End date cannot be before start date.");

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == dto.EmployeeId)
                ?? throw new NotFoundException($"Employee with id {dto.EmployeeId} was not found.");

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == dto.CourseId)
                ?? throw new NotFoundException($"Course with id {dto.CourseId} was not found.");

            var alreadyAssigned = await _context.TrainingAssignments
                .AnyAsync(a => a.EmployeeId == dto.EmployeeId && a.CourseId == dto.CourseId);

            if (alreadyAssigned)
                throw new BusinessValidationException(
                    $"Employee '{employee.EmployeeName}' is already assigned to course '{course.CourseName}'.");

            var assignment = new TrainingAssignment
            {
                Employee = employee,
                Course = course,
                StartDate = startDate,
                EndDate = endDate,
                Status = TrainingStatuses.Assigned
            };

            _context.TrainingAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Course {CourseId} assigned to employee {EmployeeId} (assignment {AssignmentId})",
                course.CourseId, employee.EmployeeId, assignment.AssignmentId);

            return ToDto(assignment);
        }

        public async Task<TrainingAssignmentResponseDto> GetByIdAsync(int id)
        {
            var assignment = await _context.TrainingAssignments
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.AssignmentId == id)
                ?? throw new NotFoundException($"Training assignment with id {id} was not found.");

            return ToDto(assignment);
        }

        public async Task<List<TrainingAssignmentResponseDto>> GetByEmployeeAsync(int employeeId)
        {
            if (!await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId))
                throw new NotFoundException($"Employee with id {employeeId} was not found.");

            var assignments = await _context.TrainingAssignments
                .AsNoTracking()
                .Include(a => a.Employee)
                .Include(a => a.Course)
                .Where(a => a.EmployeeId == employeeId)
                .OrderBy(a => a.StartDate)
                .ToListAsync();

            return assignments.Select(ToDto).ToList();
        }

        public async Task<TrainingAssignmentResponseDto> UpdateStatusAsync(int id, UpdateTrainingStatusDto dto)
        {
            var assignment = await _context.TrainingAssignments
                .Include(a => a.Employee)
                .Include(a => a.Course)
                .FirstOrDefaultAsync(a => a.AssignmentId == id)
                ?? throw new NotFoundException($"Training assignment with id {id} was not found.");

            var oldStatus = assignment.Status;
            assignment.Status = dto.Status;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Assignment {AssignmentId} status changed from {OldStatus} to {NewStatus}",
                id, oldStatus, dto.Status);

            return ToDto(assignment);
        }

        public async Task DeleteAsync(int id)
        {
            var assignment = await _context.TrainingAssignments.FirstOrDefaultAsync(a => a.AssignmentId == id)
                ?? throw new NotFoundException($"Training assignment with id {id} was not found.");

            _context.TrainingAssignments.Remove(assignment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Assignment {AssignmentId} deleted", id);
        }

        private static TrainingAssignmentResponseDto ToDto(TrainingAssignment a) => new()
        {
            AssignmentId = a.AssignmentId,
            EmployeeId = a.EmployeeId,
            EmployeeName = a.Employee.EmployeeName,
            CourseId = a.CourseId,
            CourseName = a.Course.CourseName,
            StartDate = a.StartDate,
            EndDate = a.EndDate,
            Status = a.Status
        };



    }
}
