using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Service;
using StudentSupervisorService.Models.Response.ViolationConfigResponse;
using StudentSupervisorService.Models.Request.ViolationConfigRequest;
using Microsoft.AspNetCore.Authorization;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/violation-configs")]
    [ApiController]
    [Authorize]
    public class ViolationConfigController : ControllerBase
    {
        private ViolationConfigService _service;
        public ViolationConfigController(ViolationConfigService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ViolationConfigResponse>>>> GetViolationConfigs(string sortOrder = "asc")
        {
            try
            {
                var violationConfigs = await _service.GetAllViolationConfigs(sortOrder);
                return Ok(violationConfigs);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ViolationConfigResponse>>> GetviolationConfigById(int id)
        {
            try
            {
                var violationConfig = await _service.GetViolationConfigById(id);
                return Ok(violationConfig);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchViolationConfigs(int? vioTypeId = null, int? minusPoints = null, string sortOrder = "asc")
        {
            try
            {
                var violationConfigs = await _service.SearchViolationConfigs(vioTypeId, minusPoints, sortOrder);
                return Ok(violationConfigs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ViolationConfigResponse>>> CreateViolationConfig(RequestOfViolationConfig request)
        {
            try
            {
                var createdViolationConfig = await _service.CreateViolationConfig(request);
                return createdViolationConfig == null ? NotFound() : Ok(createdViolationConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ViolationConfigResponse>>> UpdateViolationConfig(int id, RequestOfViolationConfig request)
        {
            try
            {
                var updatedViolationConfig = await _service.UpdateViolationConfig(id, request);
                return updatedViolationConfig == null ? NotFound() : Ok(updatedViolationConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ViolationConfigResponse>>> DeleteViolation(int id)
        {
            try
            {
                var deletedViolationConfig = await _service.DeleteViolationConfig(id);
                return deletedViolationConfig == null ? NoContent() : Ok(deletedViolationConfig);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ViolationConfigResponse>>>> GetViolationConfigsBySchoolId(int schoolId)
        {
            try
            {
                var violationConfigs = await _service.GetViolationConfigsBySchoolId(schoolId);
                return Ok(violationConfigs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
