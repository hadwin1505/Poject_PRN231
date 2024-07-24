using AutoMapper;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.AdminResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class AdminImplement : AdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<List<AdminResponse>>> GetAllAdmins(string sortOrder)
        {
            var response = new DataResponse<List<AdminResponse>>();

            try
            {
                var users = await _unitOfWork.Admin.GetAllAdmins();
                if (users is null || !users.Any())
                {
                    response.Message = "Danh sách Admin trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách Admin theo yêu cầu
                var userDTO = _mapper.Map<List<AdminResponse>>(users);
                if (sortOrder == "desc")
                {
                    userDTO = userDTO.OrderByDescending(r => r.AdminId).ToList();
                }
                else
                {
                    userDTO = userDTO.OrderBy(r => r.AdminId).ToList();
                }
                response.Data = userDTO;
                response.Message = "Danh sách Admin";
                response.Success = true;
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
