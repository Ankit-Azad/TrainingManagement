using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Interfaces;



namespace TrainingManagementApi.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ControllerBase
    {

        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponseDto>>> GetAll()
            => Ok(await _service.GetAllAsync());                                    // 200

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));                                 // 200 / 404

        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> Create(EmployeeRequestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.EmployeeId }, created);   // 201
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EmployeeResponseDto>> Update(int id, EmployeeRequestDto dto)
            => Ok(await _service.UpdateAsync(id, dto));                             // 200 / 404 / 400

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();                                                     // 204 / 404
        }

    }
}
