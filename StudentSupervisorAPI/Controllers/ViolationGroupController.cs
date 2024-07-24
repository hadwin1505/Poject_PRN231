using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Service;
using StudentSupervisorService.Models.Response.ViolationGroupResponse;
using StudentSupervisorService.Models.Request.ViolationGroupRequest;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/violation-groups")]
    [ApiController]
    [Authorize]
    public class ViolationGroupController : ControllerBase
    {
        private ViolationGroupService _service;
        public ViolationGroupController(ViolationGroupService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfVioGroup>>>> GetVioGroups(string sortOrder = "asc")
        {
            try
            {
                var vioGroups = await _service.GetAllVioGroups(sortOrder);
                return Ok(vioGroups);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioGroup>>> GetVioGroupById(int id)
        {
            try
            {
                var vioGroup = await _service.GetVioGroupById(id);
                return Ok(vioGroup);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVioGroups(int? schoolId = null, string? name = null, string sortOrder = "asc")
        {
            try
            {
                var vioGroup = await _service.SearchVioGroups(schoolId, name, sortOrder);
                return Ok(vioGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ResponseOfVioGroup>>> CreateVioGroup(RequestOfVioGroup request)
        {
            try
            {
                var createdVioGroup = await _service.CreateVioGroup(request);
                return createdVioGroup == null ? NotFound() : Ok(createdVioGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioGroup>>> UpdateVioGroup(int id, RequestOfVioGroup request)
        {
            try
            {
                var updatedVioGroup = await _service.UpdateVioGroup(id, request);
                return updatedVioGroup == null ? NotFound() : Ok(updatedVioGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioGroup>>> DeleteVioGroup(int id)
        {
            try
            {
                var deletedVioGroup = await _service.DeleteVioGroup(id);
                return deletedVioGroup == null ? NoContent() : Ok(deletedVioGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ResponseOfVioGroup>>>> GetVioGroupsBySchoolId(int schoolId)
        {
            try
            {
                var vioGroups = await _service.GetVioGroupsBySchoolId(schoolId);
                return Ok(vioGroups);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
