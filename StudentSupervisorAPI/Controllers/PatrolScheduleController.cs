using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.PatrolScheduleRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PatrolScheduleResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/patrol-schedules")]
    [ApiController]
    [Authorize]
    public class PatrolScheduleController : ControllerBase
    {
        private readonly PatrolScheduleService patrolScheduleService;
        public PatrolScheduleController(PatrolScheduleService patrolScheduleService)
        {
            this.patrolScheduleService = patrolScheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<PatrolScheduleResponse>>>> GetAllPatrolSchedules(string sortOrder = "asc")
        {
            try
            {
                var patrolSchedulesResponse = await patrolScheduleService.GetAllPatrolSchedules(sortOrder);
                return Ok(patrolSchedulesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<PatrolScheduleResponse>>> GetPatrolScheduleById(int id)
        {
            try
            {
                var patrolScheduleResponse = await patrolScheduleService.GetPatrolScheduleById(id);
                return Ok(patrolScheduleResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<PatrolScheduleResponse>>>> SearchPatrolSchedules(
                       int? classId,
                       int? supervisorId,
                       int? teacherId,
                       DateTime? from,
                       DateTime? to,
                       PatrolScheduleStatusEnums? status,
                       string sortOrder = "asc")
        {
            try
            {
                var patrolSchedulesResponse = await patrolScheduleService.SearchPatrolSchedules(classId, supervisorId, teacherId, from, to, status.ToString(), sortOrder);
                return Ok(patrolSchedulesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<PatrolScheduleResponse>>> CreatePatrolSchedule(PatrolScheduleCreateRequest request)
        {
            try
            {
                var patrolScheduleResponse = await patrolScheduleService.CreatePatrolSchedule(request);
                return Ok(patrolScheduleResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<PatrolScheduleResponse>>> UpdatePatrolSchedule(PatrolScheduleUpdateRequest request)
        {
            try
            {
                var patrolScheduleResponse = await patrolScheduleService.UpdatePatrolSchedule(request);
                return Ok(patrolScheduleResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<PatrolScheduleResponse>>> DeletePatrolSchedule(int id)
        {
            try
            {
                var pScheduletResponse = await patrolScheduleService.DeletePatrolSchedule(id);
                return Ok(pScheduletResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<PatrolScheduleResponse>>>> GetPatrolSchedulesBySchoolId(int schoolId)
        {
            try
            {
                var patrolSchedules = await patrolScheduleService.GetPatrolSchedulesBySchoolId(schoolId);
                return Ok(patrolSchedules);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
