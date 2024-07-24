using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.ClassGroupRequest;
using StudentSupervisorService.Models.Request.RegisteredSchoolRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ClassGroupResponse;
using StudentSupervisorService.Models.Response.RegisteredSchoolResponse;
using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/registered-schools")]
    [ApiController]
    [Authorize]
    public class RegisteredSchoolController : ControllerBase
    {
        private readonly RegisteredSchoolService registeredSchoolService;
        public RegisteredSchoolController(RegisteredSchoolService registeredSchoolService)
        {
            this.registeredSchoolService = registeredSchoolService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<RegisteredSchoolResponse>>>> GetAllRegisteredSchools(string sortOrder = "asc")
        {
            try
            {
                var registeredSchoolsResponse = await registeredSchoolService.GetAllRegisteredSchools(sortOrder);
                return Ok(registeredSchoolsResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<RegisteredSchoolResponse>>> GetRegisteredSchoolById(int id)
        {
            try
            {
                var registeredSchoolResponse = await registeredSchoolService.GetRegisteredSchoolById(id);
                return Ok(registeredSchoolResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<RegisteredSchoolResponse>>>> SearchRegisteredSchools(
                       int? schoolId,
                       DateTime? registeredDate,
                       string? description,
                       RegisteredSchoolStatusEnums? status,
                       string sortOrder)
        {
            try
            {
                var registeredSchoolsResponse = await registeredSchoolService.SearchRegisteredSchools(schoolId, registeredDate, description, status.ToString(), sortOrder);
                return Ok(registeredSchoolsResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<RegisteredSchoolResponse>>> CreateRegisteredSchool(RegisteredSchoolCreateRequest request)
        {
            try
            {
                var registeredSchoolsResponse = await registeredSchoolService.CreateRegisteredSchool(request);
                return Created("", registeredSchoolsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<RegisteredSchoolResponse>>> UpdateRegisteredSchool(RegisteredSchoolUpdateRequest request)
        {
            try
            {
                var registeredSchoolsResponse = await registeredSchoolService.UpdateRegisteredSchool(request);
                return Ok(registeredSchoolsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<RegisteredSchoolResponse>>> DeleteRegisteredSchool(int id)
        {
            try
            {
                var registeredSchoolsResponse = await registeredSchoolService.DeleteRegisteredSchool(id);
                return Ok(registeredSchoolsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<RegisteredSchoolResponse>>>> GetRegisteredSchoolsBySchoolId(int schoolId)
        {
            try
            {
                var registeredSchools = await registeredSchoolService.GetRegisteredSchoolsBySchoolId(schoolId);
                return Ok(registeredSchools);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
