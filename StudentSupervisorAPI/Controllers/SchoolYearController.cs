using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.SchoolYearRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.SchoolYearResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/school-years")]
    [ApiController]
    [Authorize]
    public class SchoolYearController : ControllerBase
    {
        private SchoolYearService _service;
        public SchoolYearController(SchoolYearService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfSchoolYear>>>> GetSchoolYears(string sortOrder = "asc")
        {
            try
            {
                var schoolYear = await _service.GetAllSchoolYears(sortOrder);
                return Ok(schoolYear);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> GetSchoolYearById(int id)
        {
            try
            {
                var staff = await _service.GetSchoolYearById(id);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchSchoolYears(short? year = null, DateTime? startDate = null, DateTime? enddate = null, string sortOrder = "asc")
        {
            try
            {
                var schoolYears = await _service.SearchSchoolYears(year, startDate, enddate, sortOrder);
                return Ok(schoolYears);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> CreateSchoolYear(RequestCreateSchoolYear request)
        {
            try
            {
                var createdSchoolYear = await _service.CreateSchoolYear(request);
                return Ok(createdSchoolYear);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> UpdateSchoolYear(int id, RequestCreateSchoolYear request)
        {
            try
            {
                var updatedSchoolYear = await _service.UpdateSchoolYear(id, request);
                return Ok(updatedSchoolYear);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfSchoolYear>>> DeleteSchoolYear(int id)
        {
            try
            {
                var deletedSchoolYear = await _service.DeleteSchoolYear(id);
                return deletedSchoolYear == null ? NoContent() : Ok(deletedSchoolYear);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ResponseOfSchoolYear>>>> GetSchoolYearsBySchoolId(int schoolId)
        {
            try
            {
                var schoolYears = await _service.GetSchoolYearBySchoolId(schoolId);
                return Ok(schoolYears);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
