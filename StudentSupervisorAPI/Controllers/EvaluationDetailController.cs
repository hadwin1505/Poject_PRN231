using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.EvaluationDetailRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.EvaluationDetailResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/evaluation-details")]
    [ApiController]
    [Authorize]
    public class EvaluationDetailController : ControllerBase
    {
        private readonly EvaluationDetailService evaluationDetailService;
        public EvaluationDetailController(EvaluationDetailService evaluationDetailService)
        {
            this.evaluationDetailService = evaluationDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<EvaluationDetailResponse>>>> GetAllEvaluationDetails(string sortOrder = "asc")
        {
            try
            {
                var evaluationDetailsResponse = await evaluationDetailService.GetAllEvaluationDetails(sortOrder);
                return Ok(evaluationDetailsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<EvaluationDetailResponse>>> GetEvaluationDetailById(int id)
        {
            try
            {
                var evaluationDetailResponse = await evaluationDetailService.GetEvaluationDetailById(id);
                return Ok(evaluationDetailResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<EvaluationDetailResponse>>>> SearchEvaluationDetails(
                       int? classId,
                       int? evaluationId,
                       EvaluationDetailStatusEnums? status,
                       string sortOrder = "asc")
        {
            try
            {
                var evaluationDetailsResponse = await evaluationDetailService.SearchEvaluationDetails(classId, evaluationId, status.ToString(), sortOrder);
                return Ok(evaluationDetailsResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<EvaluationDetailResponse>>> CreateEvaluationDetail(EvaluationDetailCreateRequest request)
        {
            try
            {
                var evaluationDetailResponse = await evaluationDetailService.CreateEvaluationDetail(request);
                return Ok(evaluationDetailResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<EvaluationDetailResponse>>> UpdateEvaluationDetail(EvaluationDetailUpdateRequest request)
        {
            try
            {
                var evaluationDetailResponse = await evaluationDetailService.UpdateEvaluationDetail(request);
                return Ok(evaluationDetailResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<EvaluationDetailResponse>>> DeleteEvaluationDetail(int id)
        {
            try
            {
                var evaluationDetailResponse = await evaluationDetailService.DeleteEvaluationDetail(id);
                return Ok(evaluationDetailResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<EvaluationDetailResponse>>>> GetEvaluationDetailsBySchoolId(int schoolId)
        {
            try
            {
                var evalDetails = await evaluationDetailService.GetEvaluationDetailsBySchoolId(schoolId);
                return Ok(evalDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
