using Microsoft.EntityFrameworkCore;
using TrainingManagementApi.Data;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Exceptions;
using TrainingManagementApi.Interfaces;
using TrainingManagementApi.Models;


namespace TrainingManagementApi.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(AppDbContext context, ILogger<EmployeeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<EmployeeResponseDto>> GetAllAsync()
        {
            var employees = await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeId)
                .ToListAsync();

            return employees.Select(ToDto).ToList();
        }

        public async Task<EmployeeResponseDto> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeId == id)
                ?? throw new NotFoundException($"Employee with id {id} was not found.");

            return ToDto(employee);
        }

        public async Task<EmployeeResponseDto> CreateAsync(EmployeeRequestDto dto)
        {
            var email = dto.Email.Trim();

            if (await _context.Employees.AnyAsync(e => e.Email == email))
                throw new BusinessValidationException($"An employee with email '{email}' already exists.");

            var employee = new Employee
            {
                EmployeeName = dto.EmployeeName.Trim(),
                Email = email,
                Department = dto.Department.Trim(),
                JoiningDate = dto.JoiningDate!.Value,
                Experience = dto.Experience
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} created", employee.EmployeeId);
            return ToDto(employee);
        }

        public async Task<EmployeeResponseDto> UpdateAsync(int id, EmployeeRequestDto dto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id)
                ?? throw new NotFoundException($"Employee with id {id} was not found.");

            var email = dto.Email.Trim();

            if (await _context.Employees.AnyAsync(e => e.Email == email && e.EmployeeId != id))
                throw new BusinessValidationException($"An employee with email '{email}' already exists.");

            employee.EmployeeName = dto.EmployeeName.Trim();
            employee.Email = email;
            employee.Department = dto.Department.Trim();
            employee.JoiningDate = dto.JoiningDate!.Value;
            employee.Experience = dto.Experience;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} updated", id);
            return ToDto(employee);
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id)
                ?? throw new NotFoundException($"Employee with id {id} was not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Employee {EmployeeId} deleted", id);
        }

        private static EmployeeResponseDto ToDto(Employee e) => new()
        {
            EmployeeId = e.EmployeeId,
            EmployeeName = e.EmployeeName,
            Email = e.Email,
            Department = e.Department,
            JoiningDate = e.JoiningDate,
            Experience = e.Experience
        };

    }
}
