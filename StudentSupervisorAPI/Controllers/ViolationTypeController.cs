using Microsoft.AspNetCore.Mvc;

using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Service;
using StudentSupervisorService.Models.Response.ViolationTypeResponse;
using StudentSupervisorService.Models.Request.ViolationTypeRequest;
using Microsoft.AspNetCore.Authorization;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/violation-types")]
    [ApiController]
    [Authorize]
    public class ViolationTypeController : ControllerBase
    {
        private ViolationTypeService _service;
        public ViolationTypeController(ViolationTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfVioType>>>> GetVioTypes(string sortOrder = "asc")
        {
            try
            {
                var vioTypes = await _service.GetAllVioTypes(sortOrder);
                return Ok(vioTypes);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioType>>> GetVioTypeById(int id)
        {
            try
            {
                var vioType = await _service.GetVioTypeById(id);
                return Ok(vioType);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchVioTypes(int? vioGroupId = null, string? name =null, string sortOrder = "asc")
        {
            try
            {
                var vioTypes = await _service.SearchVioTypes(vioGroupId,  name, sortOrder);
                return Ok(vioTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ResponseOfVioType>>> CreateVioType(RequestOfVioType request)
        {
            try
            {
                var createdVioTypes = await _service.CreateVioType(request);
                return createdVioTypes == null ? NotFound() : Ok(createdVioTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioType>>> UpdateVioType(int id, RequestOfVioType request)
        {
            try
            {
                var updatedVioTypes = await _service.UpdateVioType(id, request);
                return updatedVioTypes == null ? NotFound() : Ok(updatedVioTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfVioType>>> DeleteVioType(int id)
        {
            try
            {
                var deletedVioTypes = await _service.DeleteVioType(id);
                return deletedVioTypes == null ? NoContent() : Ok(deletedVioTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ResponseOfVioType>>>> GetViolationTypesBySchoolId(int schoolId)
        {
            try
            {
                var vioTypes = await _service.GetViolationTypesBySchoolId(schoolId);
                return Ok(vioTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
