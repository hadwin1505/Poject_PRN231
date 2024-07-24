using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.DisciplineRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.DisciplineResponse;
using StudentSupervisorService.Models.Response.ViolationResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/disciplines")]
    [ApiController]
    [Authorize]
    public class DisciplineController : ControllerBase
    {
        private readonly DisciplineService disciplineService;
        public DisciplineController(DisciplineService disciplineService)
        {
            this.disciplineService = disciplineService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<DisciplineResponse>>>> GetAllDisciplines(string sortOrder = "asc")
        {
            try
            {
                var disciplinesResponse = await disciplineService.GetAllDisciplines(sortOrder);
                return Ok(disciplinesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> GetDisciplineById(int id)
        {
            try
            {
                var disciplineResponse = await disciplineService.GetDisciplineById(id);
                return Ok(disciplineResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<DisciplineResponse>>>> SearchDisciplines(
            int? violationId,
            int? penaltyId,
            string? description,
            DateTime? startDate,
            DateTime? endDate,
            DisciplineStatusEnums? status,
            string sortOrder = "asc")
        {
            try
            {
                var disciplinesResponse = await disciplineService.SearchDisciplines(violationId, penaltyId, description, startDate, endDate, status.ToString(), sortOrder);
                return Ok(disciplinesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> CreateDiscipline(DisciplineCreateRequest request)
        {
            try
            {
                var disciplineResponse = await disciplineService.CreateDiscipline(request);
                return Ok(disciplineResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> UpdateDiscipline(DisciplineUpdateRequest request)
        {
            try
            {
                var disciplineResponse = await disciplineService.UpdateDiscipline(request);
                return Ok(disciplineResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> DeleteDiscipline(int id)
        {
            try
            {
                var disciplineResponse = await disciplineService.DeleteDiscipline(id);
                return Ok(disciplineResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<DisciplineResponse>>>> GetDisciplinesBySchoolId(int schoolId)
        {
            try
            {
                var disciplines = await disciplineService.GetDisciplinesBySchoolId(schoolId);
                return Ok(disciplines);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/executing")]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> ExecutingDiscipline(int id)
        {
            try
            {
                var discipline = await disciplineService.ExecutingDiscipline(id);
                return Ok(discipline);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}/done")]
        public async Task<ActionResult<DataResponse<DisciplineResponse>>> DoneDiscipline(int id)
        {
            try
            {
                var discipline = await disciplineService.DoneDiscipline(id);
                return Ok(discipline);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
