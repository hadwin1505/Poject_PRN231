using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.EvaluationRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.EvaluationResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/evaluations")]
    [ApiController]
    [Authorize]
    public class EvaluationController : ControllerBase
    {
        private readonly EvaluationService evaluationService;
        public EvaluationController(EvaluationService evaluationService)
        {
            this.evaluationService = evaluationService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<EvaluationResponse>>>> GetAllEvaluations(string sortOrder = "asc")
        {
            try
            {
                var evaluationsResponse = await evaluationService.GetAllEvaluations(sortOrder);
                return Ok(evaluationsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<EvaluationResponse>>> GetEvaluationById(int id)
        {
            try
            {
                var evaluationResponse = await evaluationService.GetEvaluationById(id);
                return Ok(evaluationResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<EvaluationResponse>>>> SearchEvaluations(
                       int? schoolYearId,
                       int? violationConfigId,
                       string? description,
                       DateTime? from,
                       DateTime? to,
                       short? point,
                       string sortOrder = "asc")
        {
            try
            {
                var evaluationsResponse = await evaluationService.SearchEvaluations(schoolYearId, violationConfigId, description, from, to, point, sortOrder);
                return Ok(evaluationsResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<EvaluationResponse>>> CreateEvaluation(EvaluationCreateRequest request)
        {
            try
            {
                var evaluationResponse = await evaluationService.CreateEvaluation(request);
                return Ok(evaluationResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<EvaluationResponse>>> UpdateEvaluation(EvaluationUpdateRequest request)
        {
            try
            {
                var evaluationResponse = await evaluationService.UpdateEvaluation(request);
                return Ok(evaluationResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<EvaluationResponse>>> DeleteEvaluation(int id)
        {
            try
            {
                var deleteEvaluation = await evaluationService.DeleteEvaluation(id);
                return deleteEvaluation == null ? NoContent() : Ok(deleteEvaluation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<EvaluationResponse>>>> GetEvaluationsBySchoolId(int schoolId)
        {
            try
            {
                var evaluations = await evaluationService.GetEvaluationsBySchoolId(schoolId);
                return Ok(evaluations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
