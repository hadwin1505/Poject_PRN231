using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StudentSupervisorService.Models.Request.HighSchoolRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.HighschoolResponse;
using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Models.Response.ViolationGroupResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/highschools")]
    [ApiController]
    [Authorize]
    public class HighSchoolController : ControllerBase
    {
        private HighSchoolService _service;
        public HighSchoolController(HighSchoolService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfHighSchool>>>> GetHighSchools(string sortOrder = "asc")
        {
            try
            {
                var highSchools = await _service.GetAllHighSchools(sortOrder);
                return Ok(highSchools);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfHighSchool>>> GetHighSchoolById(int id)
        {
            try
            {
                var highSchool = await _service.GetHighSchoolById(id);
                return Ok(highSchool);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts(string? code = null, string? name = null, string? city = null, string? address = null, string? phone = null, string sortOrder = "asc")
        {
            try
            {
                var highSchools = await _service.SearchHighSchools(code, name, city, address, phone, sortOrder);
                return Ok(highSchools);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<DataResponse<ResponseOfHighSchool>>> CreateHighSchool(RequestOfHighSchool request)
        {
            try
            {
                var createdHighSchool = await _service.CreateHighSchool(request);
                return Ok(createdHighSchool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfHighSchool>>> UpdateHighSchool(int id, RequestOfHighSchool request)
        {
            try
            {
                var updatedHighSchool = await _service.UpdateHighSchool(id, request);
                return Ok(updatedHighSchool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfHighSchool>>> DeleteHighSchool(int id)
        {
            try
            {
                var deletedHighSchool = await _service.DeleteHighSchool(id);
                return Ok(deletedHighSchool);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
