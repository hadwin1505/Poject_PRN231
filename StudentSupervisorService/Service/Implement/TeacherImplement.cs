using AutoMapper;
using CloudinaryDotNet.Core;
using Domain.Entity;
using Domain.Enums.Role;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.TeacherRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.TeacherResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class TeacherImplement : TeacherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TeacherImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<TeacherResponse>> CreateAccountSupervisor(RequestOfTeacher request)
        {
            var response = new DataResponse<TeacherResponse>();
            try
            {
                var isExist = await _unitOfWork.User.GetAccountByPhone(request.Phone);
                if (isExist != null)
                {
                    throw new Exception("Số điện thoại đã được sử dụng !!");
                }

                var teacher = _mapper.Map<Teacher>(request);

                teacher.User = new User
                {
                    SchoolId = request.SchoolId,
                    Code = request.Code,
                    Name = request.TeacherName,
                    // Prepend "84" if not already present
                    Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone,
                    Password = request.Password,
                    Address = request.Address,
                    RoleId = (byte)RoleAccountEnum.SUPERVISOR,
                    Status = UserStatusEnums.ACTIVE.ToString()
                };

                _unitOfWork.Teacher.Add(teacher);
                _unitOfWork.Save();

                response.Data = _mapper.Map<TeacherResponse>(teacher);
                response.Message = "Tài khoản Giám thị được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo tài khoản Giám thị không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<TeacherResponse>> CreateAccountTeacher(RequestOfTeacher request)
        {
            var response = new DataResponse<TeacherResponse>();
            try
            {
                var isExist = await _unitOfWork.User.GetAccountByPhone(request.Phone);
                if (isExist != null)
                {
                    throw new Exception("Số điện thoại đã được sử dụng !!");
                }

                var teacher = _mapper.Map<Teacher>(request);

                teacher.User = new User
                {
                    SchoolId = request.SchoolId,
                    Code = request.Code,
                    Name = request.TeacherName,
                    // Prepend "84" if not already present
                    Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone,
                    Password = request.Password,
                    Address = request.Address,
                    RoleId = (byte)RoleAccountEnum.TEACHER,
                    Status = UserStatusEnums.ACTIVE.ToString()
                };

                _unitOfWork.Teacher.Add(teacher);
                _unitOfWork.Save();

                response.Data = _mapper.Map<TeacherResponse>(teacher);
                response.Message = "Tài khoản giáo viên được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo tài khoản Giáo viên không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<TeacherResponse>> DeleteTeacher(int id)
        {
            var response = new DataResponse<TeacherResponse>();
            try
            {
                var teacher = await _unitOfWork.Teacher.GetTeacherByIdWithUser(id);
                if (teacher == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Giáo viên có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (teacher.User == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Tài khoản được liên kết cho Giáo viên có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if(teacher.User.Status == UserStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Tài khoản liên kết với Giáo viên đã bị xóa.";
                    response.Success = false;
                    return response;
                }

                teacher.User.Status = UserStatusEnums.INACTIVE.ToString();
                _unitOfWork.User.Update(teacher.User);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Giáo viên đã xóa thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Giáo viên không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<TeacherResponse>>> GetAllTeachers(string sortOrder)
        {
            var response = new DataResponse<List<TeacherResponse>>();

            try
            {
                var teachers = await _unitOfWork.Teacher.GetAllTeachers();
                if (teachers is null || !teachers.Any())
                {
                    response.Message = "Danh sách Giáo viên trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách sản phẩm theo yêu cầu
                var teacherDTO = _mapper.Map<List<TeacherResponse>>(teachers);
                if (sortOrder == "desc")
                {
                    teacherDTO = teacherDTO.OrderByDescending(r => r.TeacherId).ToList();
                }
                else
                {
                    teacherDTO = teacherDTO.OrderBy(r => r.TeacherId).ToList();
                }
                response.Data = teacherDTO;
                response.Message = "Danh sách các giáo viên";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<TeacherResponse>> GetTeacherById(int id)
        {
            var response = new DataResponse<TeacherResponse>();

            try
            {
                var teacher = await _unitOfWork.Teacher.GetTeacherById(id);
                if (teacher is null)
                {
                    throw new Exception("Giáo viên không tồn tại");
                }
                response.Data = _mapper.Map<TeacherResponse>(teacher);
                response.Message = $"TeacherId {teacher.TeacherId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<TeacherResponse>>> GetTeachersBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<TeacherResponse>>();

            try
            {
                var teachers = await _unitOfWork.Teacher.GetTeachersBySchoolId(schoolId);
                if (teachers == null || !teachers.Any())
                {
                    response.Message = "Không tìm thấy Giáo viên nào cho SchoolId được chỉ định!!";
                    response.Success = false;
                }
                else
                {
                    var teacherDTOs = _mapper.Map<List<TeacherResponse>>(teachers);
                    response.Data = teacherDTOs;
                    response.Message = "Tìm thấy Giáo viên";
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

        public async Task<DataResponse<List<TeacherResponse>>> SearchTeachers(int? schoolId, int? userId, bool sex, string sortOrder)
        {
            var response = new DataResponse<List<TeacherResponse>>();

            try
            {
                var teachers = await _unitOfWork.Teacher.SearchTeachers(schoolId, userId, sex);
                if (teachers is null || teachers.Count == 0)
                {
                    response.Message = "Không tìm thấy giáo viên nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var teacherDTO = _mapper.Map<List<TeacherResponse>>(teachers);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        teacherDTO = teacherDTO.OrderByDescending(p => p.TeacherId).ToList();
                    }
                    else
                    {
                        teacherDTO = teacherDTO.OrderBy(p => p.TeacherId).ToList();
                    }

                    response.Data = teacherDTO;
                    response.Message = "Tìm thấy Giáo viên";
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

        public async Task<DataResponse<TeacherResponse>> UpdateTeacher(int id, RequestOfTeacher request)
        {
            var response = new DataResponse<TeacherResponse>();

            try
            {
                var teacher = await _unitOfWork.Teacher.GetTeacherByIdWithUser(id);
                if (teacher == null)
                {
                    response.Message = "Không thể tìm thấy Giáo viên";
                    response.Success = false;
                    return response;
                }

                // Check if Code already exists for another teacher
                var isExistCode =  _unitOfWork.User.Find(u => u.Code == request.Code && u.UserId != teacher.UserId).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã tài khoản đã được sử dụng";
                    response.Success = false;
                    return response;
                }

                // Check if Phone already exists for another teacher
                var isExistPhone =  _unitOfWork.User.Find(u => u.Phone == request.Phone && u.UserId != teacher.UserId).FirstOrDefault();
                if (isExistPhone != null)
                {
                    response.Message = "Số điện thoại đã được sử dụng";
                    response.Success = false;
                    return response;
                }

                // Update Teacher entity
                _mapper.Map(request, teacher);

                // Update User entity
                var user = teacher.User;
                user.Name = request.TeacherName;
                // Prepend "84" if not already present
                user.Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone;
                user.Password = request.Password;
                user.Address = request.Address;
                user.Status = UserStatusEnums.ACTIVE.ToString(); 

                _unitOfWork.Teacher.Update(teacher);
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();

                response.Data = _mapper.Map<TeacherResponse>(teacher);
                response.Success = true;
                response.Message = "Cập nhật Giáo viên thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật Giáo viên không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }
    }
}
