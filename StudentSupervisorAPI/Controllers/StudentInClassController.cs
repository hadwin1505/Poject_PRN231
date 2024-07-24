using Domain.Enums.Status;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Request.StudentInClassRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentInClassResponse;
using StudentSupervisorService.Service;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/student-in-classes")]
    [ApiController]
    [Authorize]
    public class StudentInClassController : ControllerBase
    {
        private readonly StudentInClassService studentInClassService;
        public StudentInClassController(StudentInClassService studentInClassService)
        {
            this.studentInClassService = studentInClassService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<StudentInClassResponse>>>> GetAllStudentInClasses(string sortOrder = "asc")
        {
            try
            {
                var studentInClassesResponse = await studentInClassService.GetAllStudentInClass(sortOrder);
                return Ok(studentInClassesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<StudentInClassResponse>>> GetStudentInClassById(int id)
        {
            try
            {
                var studentInClassResponse = await studentInClassService.GetStudentInClassById(id);
                return Ok(studentInClassResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<StudentInClassResponse>>>> SearchStudentInClasses(
                int? classId, 
                int? studentId, 
                DateTime? enrollDate, 
                bool? isSupervisor, 
                DateTime? startDate, 
                DateTime? endDate, 
                int? numberOfViolation, 
                StudentInClassStatusEnums? status, 
                string sortOrder)
        {
            try
            {
                var studentInClassesResponse = await studentInClassService.SearchStudentInClass(classId, studentId, enrollDate, isSupervisor, startDate, endDate, numberOfViolation, status.ToString(), sortOrder);
                return Ok(studentInClassesResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<StudentInClassResponse>>> CreateStudentInClass(StudentInClassCreateRequest request)
        {
            try
            {
                var studentInClassResponse = await studentInClassService.CreateStudentInClass(request);
                return Ok(studentInClassResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<StudentInClassResponse>>> UpdateStudentInClass(StudentInClassUpdateRequest request)
        {
            try
            {
                var studentInClassResponse = await studentInClassService.UpdateStudentInClass(request);
                return Ok(studentInClassResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<StudentInClassResponse>>> DeleteStudentInClass(int id)
        {
            try
            {
                var studentInClassResponse = await studentInClassService.DeleteStudentInClass(id);
                return Ok(studentInClassResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<StudentInClassResponse>>>> GetStudentInClassesBySchoolId(int schoolId)
        {
            try
            {
                var studentInClasses = await studentInClassService.GetStudentInClassesBySchoolId(schoolId);
                return Ok(studentInClasses);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("change-class")]
        public async Task<ActionResult<DataResponse<StudentInClassResponse>>> ChangeStudentToAnotherClass(int studentInClassId, int newClassId)
        {
            try
            {
                var studentInClassResponse = await studentInClassService.ChangeStudentToAnotherClass(studentInClassId, newClassId);
                return Ok(studentInClassResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
