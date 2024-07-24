using AutoMapper;
using Domain.Entity;
using Domain.Enums.Role;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.UserRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.UserResponse;
using System.Security.Principal;


namespace StudentSupervisorService.Service.Implement
{
    public class UserImplement : UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<ResponseOfUser>> CreatePrincipal(RequestOfUser request)
        {
            var response = new DataResponse<ResponseOfUser>();

            try
            {
                var isExistPhone = await _unitOfWork.User.GetAccountByPhone(request.Phone);
                if (isExistPhone != null)
                {
                    throw new Exception("Số điện thoại đã được sử dụng!!");
                }

                var isExistCode = _unitOfWork.User.Find(u => u.Code == request.Code).FirstOrDefault();
                if (isExistCode != null)
                {
                    throw new Exception("Mã tài khoản đã được sử dụng!!");
                }

                var newPrincipal = _mapper.Map<User>(request);
                newPrincipal.RoleId = (byte)RoleAccountEnum.PRINCIPAL;
                newPrincipal.Status = UserStatusEnums.ACTIVE.ToString();

                // Prepend "84" if not already present
                newPrincipal.Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone;

                _unitOfWork.User.Add(newPrincipal);
                _unitOfWork.Save();

                var userResponse = _mapper.Map<ResponseOfUser>(newPrincipal);

                response.Data = userResponse;
                response.Message = "Tài khoản Ban giám hiệu đã được tạo thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo tài khoản Ban giám hiệu không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfUser>> CreateSchoolAdmin(RequestOfUser request)
        {
            var response = new DataResponse<ResponseOfUser>();

            try
            {
                var isExistPhone = await _unitOfWork.User.GetAccountByPhone(request.Phone);
                if (isExistPhone != null)
                {
                    throw new Exception("Số điện thoại đã được sử dụng!!");
                }

                var isExistCode = _unitOfWork.User.Find(u => u.Code == request.Code).FirstOrDefault();
                if (isExistCode != null)
                {
                    throw new Exception("Mã tài khoản đã được sử dụng!!");
                }

                var newSchoolAdmin = _mapper.Map<User>(request);
                newSchoolAdmin.RoleId = (byte)RoleAccountEnum.SCHOOL_ADMIN;
                newSchoolAdmin.Status = UserStatusEnums.ACTIVE.ToString();

                // Prepend "84" if not already present
                newSchoolAdmin.Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone;

                _unitOfWork.User.Add(newSchoolAdmin);
                _unitOfWork.Save();

                var userResponse = _mapper.Map<ResponseOfUser>(newSchoolAdmin);

                response.Data = userResponse;
                response.Message = "Tài khoản SchoolAdmin đã tạo thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo SchoolAdmin không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfUser>> DeleteUser(int userId)
        {
            var response = new DataResponse<ResponseOfUser>();
            try
            {
                var user = _unitOfWork.User.GetById(userId);
                if (user is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy tài khoản có ID: " + userId;
                    response.Success = false;
                    return response;
                }

                if (user.Status == UserStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Tài khoản đã bị xóa!!";
                    response.Success = false;
                    return response;
                }

                user.Status = UserStatusEnums.INACTIVE.ToString();
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Tài khoản đã được xóa thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa tài khoản thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response; ;
        }

        public async Task<DataResponse<List<ResponseOfUser>>> GetAllUsers(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfUser>>();

            try
            {
                var users = await _unitOfWork.User.GetAllUsers();
                if (users is null || !users.Any())
                {
                    response.Message = "Danh sách Tài khoản trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách User theo yêu cầu
                var userDTO = _mapper.Map<List<ResponseOfUser>>(users);
                if (sortOrder == "desc")
                {
                    userDTO = userDTO.OrderByDescending(r => r.UserId).ToList();
                }
                else
                {
                    userDTO = userDTO.OrderBy(r => r.UserId).ToList();
                }
                response.Data = userDTO;
                response.Message = "Danh sách các tài khoản";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfUser>> GetUserById(int id)
        {
            var response = new DataResponse<ResponseOfUser>();

            try
            {
                var user = await _unitOfWork.User.GetUserById(id);
                if (user is null)
                {
                    throw new Exception("Tài khoản không tồn tại");
                }
                response.Data = _mapper.Map<ResponseOfUser>(user);
                response.Message = $"UserId {user.UserId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfUser>>> GetUsersBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ResponseOfUser>>();

            try
            {
                var users = await _unitOfWork.User.GetUsersBySchoolId(schoolId);
                if (users == null || !users.Any())
                {
                    response.Message = "Không tìm thấy tài khoản người dùng nào cho SchoolId được chỉ định.";
                    response.Success = false;
                }
                else
                {
                    var userDTOs = _mapper.Map<List<ResponseOfUser>>(users);
                    response.Data = userDTOs;
                    response.Message = "Tìm thấy tài khoản người dùng";
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

        public async Task<DataResponse<List<ResponseOfUser>>> SearchUsers(int? schoolId, int? role, string? code, string? name, string? phone, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfUser>>();

            try
            {
                var users = await _unitOfWork.User.SearchUsers( schoolId,role, code, name, phone);
                if (users is null || users.Count == 0)
                {
                    response.Message = "Không tìm thấy Tài khoản Người dùng nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var userDTO = _mapper.Map<List<ResponseOfUser>>(users);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        userDTO = userDTO.OrderByDescending(p => p.UserId).ToList();
                    }
                    else
                    {
                        userDTO = userDTO.OrderBy(p => p.UserId).ToList();
                    }

                    response.Data = userDTO;
                    response.Message = "Tìm thấy tài khoản người dùng";
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

        public async Task<DataResponse<ResponseOfUser>> UpdateUser(int id, RequestOfUser request)
        {
            var response = new DataResponse<ResponseOfUser>();

            try
            {
                var user = _unitOfWork.User.GetById(id);
                if (user is null)
                {
                    response.Message = "Không thể tìm thấy tài khoản";
                    response.Success = false;
                    return response;
                }

                var isExistCode = _unitOfWork.User.Find(u => u.Code == request.Code && u.UserId != id).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã tài khoản đã được sử dụng";
                    response.Success = false;
                    return response;
                }

                var isExistPhone = _unitOfWork.User.Find(u => u.Phone == request.Phone && u.UserId != id).FirstOrDefault();
                if (isExistPhone != null)
                {
                    response.Message = "Số điện thoại đã được sử dụng";
                    response.Success = false;
                    return response;
                }


                user.SchoolId = request.SchoolId;
                user.Code = request.Code;
                user.Name = request.Name;
                // Prepend "84" if not already present
                user.Phone = request.Phone.StartsWith("84") ? request.Phone : "84" + request.Phone;
                user.Password = request.Password;
                user.Address = request.Address;
                _unitOfWork.User.Update(user);
                _unitOfWork.Save();
                response.Data = _mapper.Map<ResponseOfUser>(user);
                response.Success = true;
                response.Message = "Cập nhật tài khoản thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật tài khoản thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
