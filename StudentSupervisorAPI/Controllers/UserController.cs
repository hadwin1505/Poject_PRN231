using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Service;
using StudentSupervisorService.Models.Response.UserResponse;
using StudentSupervisorService.Models.Request.UserRequest;
using Microsoft.AspNetCore.Authorization;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private UserService _service;
        public UserController(UserService service)
        {
            _service = service;
        }
        //[Authorize(Roles = "SCHOOLADMIN")]
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ResponseOfUser>>>> GetUsers(string sortOrder = "asc")
        {
            try 
            {
                var users = await _service.GetAllUsers(sortOrder);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfUser>>> GetUserById(int id)
        {
            try
            {
                var user = await _service.GetUserById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers(int? schoolId = null,int? role = null, string? code = null, string? name = null, string? phone = null, string sortOrder = "asc")
        {
            try
            {
                var users = await _service.SearchUsers(schoolId, role, code, name, phone, sortOrder);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("principal")]
        public async Task<ActionResult<DataResponse<ResponseOfUser>>> CreatePrincipal(RequestOfUser request)
        {
            try
            {
                var createdPrincipal = await _service.CreatePrincipal(request);
                return createdPrincipal == null ? NotFound() : Ok(createdPrincipal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("school_admin")]
        public async Task<ActionResult<DataResponse<ResponseOfUser>>> CreateSchoolAdmin(RequestOfUser request)
        {
            try
            {
                var createdSchoolAdmin = await _service.CreateSchoolAdmin(request);
                return createdSchoolAdmin == null ? NotFound() : Ok(createdSchoolAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfUser>>> UpdateSchoolYear(int id, RequestOfUser request)
        {
            try
            {
                var updatedUser = await _service.UpdateUser(id, request);
                return updatedUser == null ? NotFound() : Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ResponseOfUser>>> DeleteUser(int id)
        {
            try
            {
                var deletedUser = await _service.DeleteUser(id);
                return deletedUser == null ? NoContent() : Ok(deletedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ResponseOfUser>>>> GetUsersBySchoolId(int schoolId)
        {
            try
            {
                var users = await _service.GetUsersBySchoolId(schoolId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
