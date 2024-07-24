using AutoMapper;
using Domain.Entity;
using Domain.Enums.Role;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;
using StudentSupervisorService.Models.Request.StudentSupervisorRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.StudentSupervisorResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class StudentSupervisorImplement : StudentSupervisorServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StudentSupervisorImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<StudentSupervisorResponse>> CreateAccountStudentSupervisor(StudentSupervisorRequest request)
        {
            var response = new DataResponse<StudentSupervisorResponse>();
            try
            {
                // Check if the phone number already exists
                var isExist = await _unitOfWork.User.GetAccountByPhone(request.Phone);
                if (isExist != null)
                {
                    throw new Exception("Số điện thoại đã được sử dụng !!");
                }

                var studentSupervisor = new StudentSupervisor
                {
                    StudentInClassId = request.StudentInClassId,
                    Description = request.Description,
                    User = new User
                    {
                        SchoolId = request.SchoolId,
                        Code = request.Code,
                        Name = request.SupervisorName,
                        // Prepend "84" if not already present
                        Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone,
                        Password = request.Password,
                        Address = request.Address,
                        RoleId = (byte)RoleAccountEnum.STUDENT_SUPERVISOR,
                        Status = UserStatusEnums.ACTIVE.ToString()
                    }
                };

                _unitOfWork.StudentSupervisor.Add(studentSupervisor);

                // Cập nhật Supervisor cho StudentInClass tương ứng
                var studentInClass = _unitOfWork.StudentInClass.GetById(request.StudentInClassId);
                if (studentInClass != null)
                {
                    studentInClass.IsSupervisor = true;
                    _unitOfWork.StudentInClass.Update(studentInClass);
                }

                _unitOfWork.Save();

                response.Data = _mapper.Map<StudentSupervisorResponse>(studentSupervisor);
                response.Message = "Sao đỏ được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Sao đỏ không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }



        public async Task<DataResponse<StudentSupervisorResponse>> DeleteStudentSupervisor(int id)
        {
            var response = new DataResponse<StudentSupervisorResponse>();
            try
            {
                var stuSupervisor = await _unitOfWork.StudentSupervisor.GetStudentSupervisorById(id);
                if (stuSupervisor == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Sao đỏ có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (stuSupervisor.User == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy tài khoản được liên kết cho Sao đỏ có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (stuSupervisor.User.Status == UserStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Tài khoản liên kết với Sao đỏ đã bị xóa.";
                    response.Success = false;
                    return response;
                }

                stuSupervisor.User.Status = UserStatusEnums.INACTIVE.ToString();
                _unitOfWork.User.Update(stuSupervisor.User);

                // Cập nhật Supervisor cho StudentInClass tương ứng
                var studentInClass = _unitOfWork.StudentInClass.GetById(stuSupervisor.StudentInClassId.Value);
                if (studentInClass != null)
                {
                    studentInClass.IsSupervisor = false;
                    _unitOfWork.StudentInClass.Update(studentInClass);
                }

                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Sao đỏ đã được xóa thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Sao đỏ không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<StudentSupervisorResponse>>> GetAllStudentSupervisors(string sortOrder)
        {
            var response = new DataResponse<List<StudentSupervisorResponse>>();

            try
            {
                var stuSupervisors = await _unitOfWork.StudentSupervisor.GetAllStudentSupervisors();
                if (stuSupervisors is null || !stuSupervisors.Any())
                {
                    response.Message = "Danh sách Sao đỏ trống!!";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách sản phẩm theo yêu cầu
                var stuSuperDTO = _mapper.Map<List<StudentSupervisorResponse>>(stuSupervisors);
                if (sortOrder == "desc")
                {
                    stuSuperDTO = stuSuperDTO.OrderByDescending(r => r.StudentSupervisorId).ToList();
                }
                else
                {
                    stuSuperDTO = stuSuperDTO.OrderBy(r => r.StudentSupervisorId).ToList();
                }
                response.Data = stuSuperDTO;
                response.Message = "Danh sách các Sao đỏ";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<StudentSupervisorResponse>> GetStudentSupervisorById(int id)
        {
            var response = new DataResponse<StudentSupervisorResponse>();

            try
            {
                var stuSupervisor = await _unitOfWork.StudentSupervisor.GetStudentSupervisorById(id);
                if (stuSupervisor is null)
                {
                    throw new Exception("Sao đỏ không tồn tại");
                }
                response.Data = _mapper.Map<StudentSupervisorResponse>(stuSupervisor);
                response.Message = $"StudentSupervisorId {stuSupervisor.StudentSupervisorId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<StudentSupervisorResponse>>> GetStudentSupervisorsBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<StudentSupervisorResponse>>();
            try
            {
                var studentSupervisors = await _unitOfWork.StudentSupervisor.GetStudentSupervisorsBySchoolId(schoolId);
                if (studentSupervisors == null || !studentSupervisors.Any())
                {
                    response.Message = "Không tìm thấy Sao đỏ nào cho SchoolId được chỉ định";
                    response.Success = false;
                }
                else
                {
                    var stuSupervisorDTOs = _mapper.Map<List<StudentSupervisorResponse>>(studentSupervisors);
                    response.Data = stuSupervisorDTOs;
                    response.Message = "Tìm thấy Sao đỏ";
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

        public async Task<DataResponse<List<StudentSupervisorResponse>>> SearchStudentSupervisors(int? userId, int? studentInClassId, string sortOrder)
        {
            var response = new DataResponse<List<StudentSupervisorResponse>>();

            try
            {
                var stuSupervisors = await _unitOfWork.StudentSupervisor.SearchStudentSupervisors(userId, studentInClassId);
                if (stuSupervisors is null || stuSupervisors.Count == 0)
                {
                    response.Message = "Không tìm thấy Sao đỏ nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var stuSupervisorDTO = _mapper.Map<List<StudentSupervisorResponse>>(stuSupervisors);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        stuSupervisorDTO = stuSupervisorDTO.OrderByDescending(p => p.StudentSupervisorId).ToList();
                    }
                    else
                    {
                        stuSupervisorDTO = stuSupervisorDTO.OrderBy(p => p.StudentSupervisorId).ToList();
                    }

                    response.Data = stuSupervisorDTO;
                    response.Message = "Tìm thấy Sao đỏ";
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

        public async Task<DataResponse<StudentSupervisorResponse>> UpdateStudentSupervisor(int id, StudentSupervisorRequest request)
        {
            var response = new DataResponse<StudentSupervisorResponse>();

            try
            {
                var studentSupervisor = await _unitOfWork.StudentSupervisor.GetStudentSupervisorById(id);
                if (studentSupervisor == null)
                {
                    response.Message = "Không thể tìm thấy Sao đỏ";
                    response.Success = false;
                    return response;
                }

                // Check if Code already exists for another StudentSupervisor
                var isExistCode = _unitOfWork.User.Find(u => u.Code == request.Code && u.UserId != studentSupervisor.UserId).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã tài khoản đã được sử dụng!!";
                    response.Success = false;
                    return response;
                }

                // Check if Phone already exists for another StudentSupervisor
                var isExistPhone = _unitOfWork.User.Find(u => u.Phone == request.Phone && u.UserId != studentSupervisor.UserId).FirstOrDefault();
                if (isExistPhone != null)
                {
                    response.Message = "Số điện thoại tài khoản đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                studentSupervisor.StudentInClassId = request.StudentInClassId;
                studentSupervisor.Description = request.Description;

                var user = studentSupervisor.User;
                user.SchoolId = request.SchoolId;
                user.Code = request.Code;
                user.Name = request.SupervisorName;
                // Prepend "84" if not already present
                user.Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone;
                user.Password = request.Password; 
                user.Address = request.Address;
                user.Status = UserStatusEnums.ACTIVE.ToString();

                _unitOfWork.StudentSupervisor.Update(studentSupervisor);
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();

                response.Data = _mapper.Map<StudentSupervisorResponse>(studentSupervisor);
                response.Success = true;
                response.Message = "Cập nhật Sao đỏ thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật Sao đỏ không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
