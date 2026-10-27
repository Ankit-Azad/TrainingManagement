using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.DTOs;
using TrainingManagementApi.Interfaces;


namespace TrainingManagementApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : ControllerBase
    {

        private readonly ICourseService _service;

        public CoursesController(ICourseService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<CourseResponseDto>>> GetAll()
            => Ok(await _service.GetAllAsync());

        // Not required by the assignment, but needed for the "Location" header of a 201 response
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseResponseDto>> GetById(int id)
            => Ok(await _service.GetByIdAsync(id));

        [HttpPost]
        public async Task<ActionResult<CourseResponseDto>> Create(CourseRequestDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CourseResponseDto>> Update(int id, CourseRequestDto dto)
        => Ok(await _service.UpdateAsync(id, dto));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

    }
}
