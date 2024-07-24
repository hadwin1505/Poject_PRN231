using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.ClassRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ClassResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/classes")]
    [ApiController]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly ClassService classService;
        public ClassController(ClassService classService)
        {
            this.classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ClassResponse>>>> GetAllClasses(string sortOrder = "asc")
        {
            try
            {
                var classesResponse = await classService.GetAllClasses(sortOrder);
                return Ok(classesResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<ClassResponse>>> GetClassById(int id)
        {
            try
            {
                var classReponse = await classService.GetClassById(id);
                return Ok(classReponse);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<ClassResponse>>>> SearchClasses(
            int? schoolYearId, 
            int? classGroupId, 
            string? code, 
            string? name, 
            int? grade,
            int? totalPoint,
            string? sortOrder)
        {
            try
            {
                var classesReponse = await classService.SearchClasses(schoolYearId, classGroupId, code, grade, name, totalPoint, sortOrder);
                return Ok(classesReponse);
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<ClassResponse>>> CreateClass(ClassCreateRequest request)
        {
            try
            {
                var classResponse = await classService.CreateClass(request);
                return Created("", classResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<ClassResponse>>> UpdateClass(ClassUpdateRequest request)
        {
            try
            {
                var classResponse = await classService.UpdateClass(request);
                return Ok(classResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<ClassResponse>>> DeleteClass(int id)
        {
            try
            {
                var classResponse = await classService.DeleteClass(id);
                return Ok(classResponse);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<ClassResponse>>>> GetClassesBySchoolId(int schoolId)
        {
            try
            {
                var classes = await classService.GetClassesBySchoolId(schoolId);
                return Ok(classes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
