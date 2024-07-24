using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Service;
using StudentSupervisorService.Models.Response.StudentResponse;
using StudentSupervisorService.Models.Request.StudentRequest;
using Microsoft.AspNetCore.Authorization;

namespace StudentSupervisorAPI.Controllers
{
    [Route("api/students")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly StudentService studentService;
        public StudentController(StudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<DataResponse<List<StudentResponse>>>> GetAllStudents(string sortOrder = "asc")
        {
            try
            {
                var studentsResponse = await studentService.GetAllStudents(sortOrder);
                return Ok(studentsResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponse<StudentResponse>>> GetStudentById(int id)
        {
            try
            {
                var studentReponse = await studentService.GetStudentById(id);
                return Ok(studentReponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<DataResponse<List<StudentResponse>>>> SearchStudents(
            int? schoolId,
            string? code,
            string? name,
            bool? sex,
            DateTime? birthday,
            string? address,
            string? phone,
            string? sortOrder)
        {
            try
            {
                var studentsResponse = await studentService.SearchStudents(schoolId, code, name, sex, birthday, address, phone, sortOrder);
                return Ok(studentsResponse);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<DataResponse<StudentResponse>>> CreateStudent(StudentCreateRequest request)
        {
            try
            {
                var studentResponse = await studentService.CreateStudent(request);
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DataResponse<StudentResponse>>> UpdateStudent(StudentUpdateRequest request)
        {
            try
            {
                var studentResponse = await studentService.UpdateStudent(request);
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DataResponse<StudentResponse>>> DeleteStudent(int id)
        {
            try
            {
                var studentResponse = await studentService.DeleteStudent(id);
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
            }
        }

        [HttpGet("school/{schoolId}")]
        public async Task<ActionResult<DataResponse<List<StudentResponse>>>> GetStudentsBySchoolId(int schoolId)
        {
            try
            {
                var students = await studentService.GetStudentsBySchoolId(schoolId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("upload")]
        //public async Task<ActionResult<DataResponse<List<string>>>> UploadImage(List<IFormFile> listImage)
        //{
        //    try
        //    {
        //        var results = await imageUrlService.UploadImage(listImage);

        //        // Extract URLs from the two images
        //        var urls = results.Select(result => result.SecureUrl.ToString()).ToList();
        //        return Ok(urls);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("publicId")]
        //public async Task<ActionResult<DataResponse<string>>> DeleteImage(string publicId)
        //{
        //    try
        //    {
        //        var result = await imageUrlService.DeleteImage(publicId);
        //        if (result.Error != null)
        //        {
        //            return BadRequest(result.Error.Message);
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
