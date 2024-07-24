using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.PenaltyRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PenaltyResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/penalties")]
    [ApiController]
    [Authorize]
    public class PenaltyController : ControllerBase
    {
        private readonly PenaltyService penaltyService;
        public PenaltyController(PenaltyService penaltyService)
        {
            this.penaltyService = penaltyService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<PenaltyResponse>>>> GetAllPenalties(string sortOrder = "asc")
        {
            try
            {
                var penaltiesResponse = await penaltyService.GetAllPenalties(sortOrder);
                return Ok(penaltiesResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<PenaltyResponse>>> GetPenaltyById(int id)
        {
            try
            {
                var penaltyReponse = await penaltyService.GetPenaltyById(id);
                return Ok(penaltyReponse);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<PenaltyResponse>>>> SearchPenalties(
                int? schoolId, 
                string? name, 
                string? description, 
                PenaltyStatusEnums? status,
                string? sortOrder)
        {
            try
            {
                var penaltiesReponse = await penaltyService.SearchPenalties(schoolId, name, description, status.ToString(),sortOrder);
                return Ok(penaltiesReponse);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<PenaltyResponse>>> CreatePenalty(PenaltyCreateRequest penaltyRequest)
        {
            try
            {
                var penaltyResponse = await penaltyService.CreatePenalty(penaltyRequest);
                return Ok(penaltyResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<PenaltyResponse>>> UpdatePenalty(PenaltyUpdateRequest penaltyRequest)
        {
            try
            {
                var penaltyResponse = await penaltyService.UpdatePenalty(penaltyRequest);
                return Ok(penaltyResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<PenaltyResponse>>> DeletePenalty(int id)
        {
            try
            {
                var penaltyResponse = await penaltyService.DeletePenalty(id);
                return Ok(penaltyResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<PenaltyResponse>>>> GetPelnaltiesBySchoolId(int schoolId)
        {
            try
            {
                var penalties = await penaltyService.GetPenaltiesBySchoolId(schoolId);
                return Ok(penalties);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
