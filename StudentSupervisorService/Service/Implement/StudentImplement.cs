using AutoMapper;
using Azure.Core;
using Domain.Entity;
using Infrastructures.Interfaces.IUnitOfWork;
using Infrastructures.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using StudentSupervisorService.Models.Request.StudentRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentResponse;

namespace StudentSupervisorService.Service.Implement
{
    public class StudentImplement : StudentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StudentImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DataResponse<List<StudentResponse>>> GetAllStudents(string sortOrder)
        {
            var response = new DataResponse<List<StudentResponse>>();
            try
            {
                var studentEntities = await unitOfWork.Student.GetAllStudents();
                if (studentEntities is null || !studentEntities.Any())
                {
                    response.Message = "Danh sách học sinh trống!!";
                    response.Success = true;
                    return response;
                }

                studentEntities = sortOrder == "desc"
                    ? studentEntities.OrderByDescending(r => r.StudentId).ToList()
                    : studentEntities.OrderBy(r => r.StudentId).ToList();

                response.Data = mapper.Map<List<StudentResponse>>(studentEntities);
                response.Message = "Danh sách các học sinh";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<StudentResponse>> GetStudentById(int id)
        {
            var response = new DataResponse<StudentResponse>();
            try
            {
                var studentEntity = await unitOfWork.Student.GetStudentById(id);
                if (studentEntity == null)
                {
                    response.Message = "Không tìm thấy học sinh!!";
                    response.Success = false;
                    return response;
                }

                response.Data = mapper.Map<StudentResponse>(studentEntity);
                response.Message = "Tìm thấy học sinh";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<StudentResponse>>> SearchStudents(int? schoolId, string? code, string? name, bool? sex, DateTime? birthday, string? address, string? phone, string sortOrder)
        {
            var response = new DataResponse<List<StudentResponse>>();

            try
            {
                var studentEntities = await unitOfWork.Student.SearchStudents(schoolId, code, name, sex, birthday, address, phone);
                if (studentEntities is null || studentEntities.Count == 0)
                {
                    response.Message = "Không có Học sinh nào phù hợp với tiêu chí tìm kiếm";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        studentEntities = studentEntities.OrderByDescending(r => r.StudentId).ToList();
                    }
                    else
                    {
                        studentEntities = studentEntities.OrderBy(r => r.StudentId).ToList();
                    }
                    response.Data = mapper.Map<List<StudentResponse>>(studentEntities);
                    response.Message = "Danh sách học sinh";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<StudentResponse>> CreateStudent(StudentCreateRequest request)
        {
            var response = new DataResponse<StudentResponse>();
            try
            {
                var isExistCode = unitOfWork.Student.Find(s => s.Code == request.Code).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã học sinh đã được sử dụng";
                    response.Success = false;
                    return response;
                }

                var studentEntity = new Student
                {
                    SchoolId = request.SchoolId,
                    Code = request.Code,
                    Name = request.Name,
                    Sex = request.Sex,
                    Birthday = request.Birthday,
                    Address = request.Address,
                    Phone = request.Phone
                };

                var created = await unitOfWork.Student.CreateStudent(studentEntity);

                response.Data = mapper.Map<StudentResponse>(created);
                response.Message = "Học sinh đã được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo học sinh thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<StudentResponse>> UpdateStudent(StudentUpdateRequest request)
        {
            var response = new DataResponse<StudentResponse>();
            try
            {
                var existingStudent = await unitOfWork.Student.GetStudentById(request.StudentId);
                if (existingStudent == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Học sinh";
                    response.Success = false;
                    return response;
                }

                var isExistCode = unitOfWork.Student.Find(s => s.Code == request.Code && s.StudentId != request.StudentId).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã học sinh đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                existingStudent.SchoolId = request.SchoolId ?? existingStudent.SchoolId;
                existingStudent.Code = request.Code ?? existingStudent.Code;
                existingStudent.Name = request.Name ?? existingStudent.Name;
                existingStudent.Sex = request.Sex ?? existingStudent.Sex;
                existingStudent.Birthday = request.Birthday ?? existingStudent.Birthday;
                existingStudent.Address = request.Address ?? existingStudent.Address;
                existingStudent.Phone = request.Phone ?? existingStudent.Phone;

                await unitOfWork.Student.UpdateStudent(existingStudent);

                response.Data = mapper.Map<StudentResponse>(existingStudent);
                response.Message = "Học sinh được cập nhật thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Cập nhật học sinh thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<StudentResponse>> DeleteStudent(int studentId)
        {
            var response = new DataResponse<StudentResponse>();
            try
            {
                var existingStudent = await unitOfWork.Student.GetStudentById(studentId);
                if (existingStudent == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Học sinh";
                    response.Success = false;
                    return response;
                }
                await unitOfWork.Student.DeleteStudent(studentId);
                response.Data = "Empty";
                response.Message = "Học sinh đã được xóa thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Xóa học sinh không thành công.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<StudentResponse>>> GetStudentsBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<StudentResponse>>();

            try
            {
                var students = await unitOfWork.Student.GetStudentsBySchoolId(schoolId);
                if (students == null || !students.Any())
                {
                    response.Message = "Không tìm thấy Học sinh nào cho SchoolId được chỉ định.";
                    response.Success = false;
                }
                else
                {
                    var studentDTO = mapper.Map<List<StudentResponse>>(students);
                    response.Data = studentDTO;
                    response.Message = "Tìm thấy Học sinh";
                    response.Success = true;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }
    }
}
