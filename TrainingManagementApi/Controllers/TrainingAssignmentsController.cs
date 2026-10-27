using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Interfaces;

namespace TrainingManagementApi.Controllers
{
    [ApiController]
    [Route("api/trainingassignments")]
    public class TrainingAssignmentsController : ControllerBase
    {
        private readonly ITrainingAssignmentService _service;

        public TrainingAssignmentsController(ITrainingAssignmentService service) => _service = service;

        // View all trainings for an employee  ->  GET /api/employees/{employeeId}/trainings
        [HttpGet("~/api/employees/{employeeId:int}/trainings")]
        public async Task<ActionResult<List<TrainingAssignmentResponseDto>>> GetByEmployee(int employeeId)
            => Ok(await _service.GetByEmployeeAsync(employeeId));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TrainingAssignmentResponseDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        // Assign a course to an employee
        [HttpPost]
        public async Task<ActionResult<TrainingAssignmentResponseDto>> Assign(AssignTrainingDto dto)
        {
            var created = await _service.AssignAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.AssignmentId }, created);
        }

        // Update training status
        [HttpPut("{id:int}/status")]
        public async Task<ActionResult<TrainingAssignmentResponseDto>> UpdateStatus(int id, UpdateTrainingStatusDto dto)
            => Ok(await _service.UpdateStatusAsync(id, dto));

        // Delete a training assignment
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
