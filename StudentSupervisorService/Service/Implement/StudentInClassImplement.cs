using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.StudentInClassRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentInClassResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class StudentInClassImplement : StudentInClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentInClassImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<StudentInClassResponse>>> GetAllStudentInClass(string sortOrder)
        {
            var response = new DataResponse<List<StudentInClassResponse>>();
            try
            {

                var studentInClassEntities = await _unitOfWork.StudentInClass.GetAllStudentInClass();
                if (studentInClassEntities is null || !studentInClassEntities.Any())
                {
                    response.Message = "Danh sách StudentInClass trống!!";
                    response.Success = true;
                    return response;
                }

                studentInClassEntities = sortOrder == "desc"
                    ? studentInClassEntities.OrderByDescending(r => r.StudentInClassId).ToList()
                    : studentInClassEntities.OrderBy(r => r.StudentInClassId).ToList();

                response.Data = _mapper.Map<List<StudentInClassResponse>>(studentInClassEntities);
                response.Message = "Danh sách các StudentInClass";
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

        public async Task<DataResponse<StudentInClassResponse>> GetStudentInClassById(int id)
        {
            var response = new DataResponse<StudentInClassResponse>();
            try
            {
                var studentInClassEntity = await _unitOfWork.StudentInClass.GetStudentInClassById(id);
                if (studentInClassEntity == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy StudentInClass !!";
                    response.Success = false;
                    return response;
                }
                response.Data = _mapper.Map<StudentInClassResponse>(studentInClassEntity);
                response.Message = "Tìm thấy StudentInClass";
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

        public async Task<DataResponse<List<StudentInClassResponse>>> SearchStudentInClass(int? classId, int? studentId, DateTime? enrollDate, bool? isSupervisor, DateTime? startDate, DateTime? endDate, int? numberOfViolation, string? status, string sortOrder)
        {
            var response = new DataResponse<List<StudentInClassResponse>>();

            try
            {
                var studentInClassEntities = await _unitOfWork.StudentInClass.SearchStudentInClass(classId, studentId, enrollDate, isSupervisor, startDate, endDate, numberOfViolation, status);
                if (studentInClassEntities is null || studentInClassEntities.Count == 0)
                {
                    response.Message = "Không có StudentInClass nào phù hợp với tiêu chí tìm kiếm!!";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        studentInClassEntities = studentInClassEntities.OrderByDescending(r => r.StudentInClassId).ToList();
                    }
                    else
                    {
                        studentInClassEntities = studentInClassEntities.OrderBy(r => r.StudentInClassId).ToList();
                    }
                    response.Data = _mapper.Map<List<StudentInClassResponse>>(studentInClassEntities);
                    response.Message = "Danh sách StudentInClass";
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

        public async Task<DataResponse<StudentInClassResponse>> CreateStudentInClass(StudentInClassCreateRequest request)
        {
            var response = new DataResponse<StudentInClassResponse>();
            try
            {
                if (await _unitOfWork.StudentInClass.IsStudentEnrolledInAnyClass(request.StudentId))
                {
                    response.Message = "Học sinh đã được ghi danh vào một lớp học khác.";
                    response.Success = false;
                    return response;
                }

                var studentInClassEntity = new StudentInClass
                {
                    ClassId = request.ClassId,
                    StudentId = request.StudentId,
                    EnrollDate = request.EnrollDate,
                    IsSupervisor = false,
                    StartDate = DateTime.Now,
                    EndDate = request.EndDate,
                    NumberOfViolation = request.NumberOfViolation,
                    Status = StudentInClassStatusEnums.ENROLLED.ToString()
                };

                var created = await _unitOfWork.StudentInClass.CreateStudentInClass(studentInClassEntity);

                response.Data = _mapper.Map<StudentInClassResponse>(created);
                response.Message = "StudentInClass được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo StudentInClass không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
        public async Task<DataResponse<StudentInClassResponse>> UpdateStudentInClass(StudentInClassUpdateRequest request)
        {
            var response = new DataResponse<StudentInClassResponse>();
            try
            {
                var existingStudentInClass = await _unitOfWork.StudentInClass.GetStudentInClassById(request.StudentInClassId);
                if (existingStudentInClass == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy StudentInClass !!";
                    response.Success = false;
                    return response;
                }

                existingStudentInClass.ClassId = request.ClassId ?? existingStudentInClass.ClassId;
                existingStudentInClass.StudentId = request.StudentId ?? existingStudentInClass.StudentId;
                existingStudentInClass.EnrollDate = request.EnrollDate ?? existingStudentInClass.EnrollDate;
                existingStudentInClass.IsSupervisor = request.IsSupervisor ?? existingStudentInClass.IsSupervisor;
                existingStudentInClass.StartDate = request.StartDate ?? existingStudentInClass.StartDate;
                existingStudentInClass.EndDate = request.EndDate ?? existingStudentInClass.EndDate;
                existingStudentInClass.NumberOfViolation = request.NumberOfViolation ?? existingStudentInClass.NumberOfViolation;

                await _unitOfWork.StudentInClass.UpdateStudentInClass(existingStudentInClass);

                response.Data = _mapper.Map<StudentInClassResponse>(existingStudentInClass);
                response.Message = "StudentInClass được cập nhật thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Cập nhật StudentInClass không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<StudentInClassResponse>> DeleteStudentInClass(int id)
        {
            var response = new DataResponse<StudentInClassResponse>();
            try
            {
                var existingStudentInClass = await _unitOfWork.StudentInClass.GetStudentInClassById(id);
                if (existingStudentInClass == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy StudentInClass !!";
                    response.Success = false;
                    return response;
                }

                if (existingStudentInClass.Status == StudentInClassStatusEnums.UNENROLLED.ToString())
                {
                    response.Data = null;
                    response.Message = "StudentInClass đã bị xóa!!";
                    response.Success = false;
                    return response;
                }

                await _unitOfWork.StudentInClass.DeleteStudentInClass(id);
                response.Data = "Empty";
                response.Message = "StudentInClass đã được xóa thành công";
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

        public async Task<DataResponse<StudentInClassResponse>> ChangeStudentToAnotherClass(int studentInClassId, int newClassId)
        {
            var response = new DataResponse<StudentInClassResponse>();
            try
            {
                var existingStudentInClass = await _unitOfWork.StudentInClass.GetStudentInClassById(studentInClassId);
                if (existingStudentInClass == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy StudentInClass !!";
                    response.Success = false;
                    return response;
                }

                // Tạo mới một bản StudentInClass cho lớp mới mà học sinh chuyển vào
                var newStudentInClass = new StudentInClass
                {
                    ClassId = newClassId,
                    StudentId = existingStudentInClass.StudentId,
                    EnrollDate = DateTime.Now,
                    IsSupervisor = existingStudentInClass.IsSupervisor,
                    StartDate = DateTime.Now,
                    EndDate = null,
                    NumberOfViolation = existingStudentInClass.NumberOfViolation,
                    Status = StudentInClassStatusEnums.ENROLLED.ToString()
                };

                // Cập nhật lại thông tin học sinh trong lớp cũ để biết được rằng học sinh đó đã không còn trong lớp đó
                existingStudentInClass.Status = StudentInClassStatusEnums.UNENROLLED.ToString();
                existingStudentInClass.EndDate = DateTime.Now;

                await _unitOfWork.StudentInClass.UpdateStudentInClass(existingStudentInClass);
                await _unitOfWork.StudentInClass.CreateStudentInClass(newStudentInClass);

                response.Data = _mapper.Map<StudentInClassResponse>(newStudentInClass);
                response.Message = "Học sinh thay đổi lớp thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Thay đổi lớp thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<StudentInClassResponse>>> GetStudentInClassesBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<StudentInClassResponse>>();
            try
            {
                var studentInClasses = await _unitOfWork.StudentInClass.GetStudentInClassesBySchoolId(schoolId);
                if (studentInClasses == null || !studentInClasses.Any())
                {
                    response.Message = "Không tìm thấy StudentInClasses nào cho SchoolId được chỉ định!!";
                    response.Success = false;
                }
                else
                {
                    var studentInClassDTOs = _mapper.Map<List<StudentInClassResponse>>(studentInClasses);
                    response.Data = studentInClassDTOs;
                    response.Message = "Tìm thấy StudentInClasses";
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
