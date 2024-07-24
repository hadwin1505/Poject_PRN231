using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using Infrastructures.Repository.UnitOfWork;
using StudentSupervisorService.Models.Request.ViolationGroupRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ViolationGroupResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class ViolationGroupImplement : ViolationGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ViolationGroupImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<ResponseOfVioGroup>> CreateVioGroup(RequestOfVioGroup request)
        {
            var response = new DataResponse<ResponseOfVioGroup>();

            try
            {
                var isExistCode = _unitOfWork.ViolationGroup.Find(s => s.Code == request.VioGroupCode).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã Nhóm vi phạm đã được sử dụng!!";
                    response.Success = false;
                    return response;
                }

                var createvioGroup = new ViolationGroup
                {
                    SchoolId = request.SchoolId,
                    Code = request.VioGroupCode,
                    Name = request.VioGroupName,
                    Description = request.Description,
                    Status = ViolationGroupStatusEnums.ACTIVE.ToString()
                };
                _unitOfWork.ViolationGroup.Add(createvioGroup);
                _unitOfWork.Save();
                var created = await _unitOfWork.ViolationGroup.GetViolationGroupById(createvioGroup.ViolationGroupId);
                response.Data = _mapper.Map<ResponseOfVioGroup>(created);
                response.Message = "Tạo Nhóm vi phạm thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Nhóm vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfVioGroup>> DeleteVioGroup(int id)
        {
            var response = new DataResponse<ResponseOfVioGroup>();
            try
            {
                var vioGroup = _unitOfWork.ViolationGroup.GetById(id);
                if (vioGroup is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Nhóm vi phạm có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (vioGroup.Status == ViolationGroupStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Nhóm vi phạm đã bị xóa.";
                    response.Success = false;
                    return response;
                }

                vioGroup.Status = ViolationGroupStatusEnums.INACTIVE.ToString();
                _unitOfWork.ViolationGroup.Update(vioGroup);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Nhóm vi phạm đã được xóa thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa nhóm vi phạm không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ResponseOfVioGroup>>> GetAllVioGroups(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfVioGroup>>();

            try
            {
                var vioGroup = await _unitOfWork.ViolationGroup.GetAllViolationGroups();
                if (vioGroup is null || !vioGroup.Any())
                {
                    response.Message = "Danh sách Nhóm vi phạm trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách Violation Group theo yêu cầu
                var vioGroupDTO = _mapper.Map<List<ResponseOfVioGroup>>(vioGroup);
                if (sortOrder == "desc")
                {
                    vioGroupDTO = vioGroupDTO.OrderByDescending(r => r.ViolationGroupId).ToList();
                }
                else
                {
                    vioGroupDTO = vioGroupDTO.OrderBy(r => r.ViolationGroupId).ToList();
                }
                response.Data = vioGroupDTO;
                response.Message = "Danh sách các nhóm vi phạm";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfVioGroup>> GetVioGroupById(int id)
        {
            var response = new DataResponse<ResponseOfVioGroup>();

            try
            {
                var vioGroup = await _unitOfWork.ViolationGroup.GetViolationGroupById(id);
                if (vioGroup is null)
                {
                    throw new Exception("Nhóm vi phạm không tồn tại");
                }
                response.Data = _mapper.Map<ResponseOfVioGroup>(vioGroup);
                response.Message = $"ViolationGroupId {vioGroup.ViolationGroupId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfVioGroup>>> GetVioGroupsBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ResponseOfVioGroup>>();
            try
            {
                var vioGroups = await _unitOfWork.ViolationGroup.GetViolationGroupBySchoolId(schoolId);
                if(vioGroups == null || !vioGroups.Any())
                {
                    response.Message = "Không tìm thấy Nhóm vi phạm nào cho SchoolId được chỉ định!!";
                    response.Success = false;
                }
                else
                {
                    var violationGroupDTOs = _mapper.Map<List<ResponseOfVioGroup>>(vioGroups);
                    response.Data = violationGroupDTOs;
                    response.Message = "Nhóm vi phạm được tìm thấy";
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

        public async Task<DataResponse<List<ResponseOfVioGroup>>> SearchVioGroups(int? schoolId, string? name, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfVioGroup>>();

            try
            {
                var vioGroups = await _unitOfWork.ViolationGroup.SearchViolationGroups(schoolId, name);
                if (vioGroups is null || vioGroups.Count == 0)
                {
                    response.Message = "Không tìm thấy Nhóm vi phạm nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var vioGroupDTO = _mapper.Map<List<ResponseOfVioGroup>>(vioGroups);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        vioGroupDTO = vioGroupDTO.OrderByDescending(p => p.ViolationGroupId).ToList();
                    }
                    else
                    {
                        vioGroupDTO = vioGroupDTO.OrderBy(p => p.ViolationGroupId).ToList();
                    }

                    response.Data = vioGroupDTO;
                    response.Message = "Nhóm vi phạm được tìm thấy";
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

        public async Task<DataResponse<ResponseOfVioGroup>> UpdateVioGroup(int id, RequestOfVioGroup request)
        {
            var response = new DataResponse<ResponseOfVioGroup>();

            try
            {
                var vioGroup = await _unitOfWork.ViolationGroup.GetViolationGroupById(id);
                if (vioGroup is null)
                {
                    response.Message = "Không thể tìm thấy Nhóm vi phạm";
                    response.Success = false;
                    return response;
                }

                var isExistCode = _unitOfWork.ViolationGroup.Find(s => s.Code == request.VioGroupCode).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã nhóm vi phạm đã được sử dụng!!";
                    response.Success = false;
                    return response;
                }

                vioGroup.SchoolId = request.SchoolId;
                vioGroup.Code = request.VioGroupCode;
                vioGroup.Name = request.VioGroupName;
                vioGroup.Description = request.Description;

                _unitOfWork.ViolationGroup.Update(vioGroup);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfVioGroup>(vioGroup);
                response.Success = true;
                response.Message = "Cập nhật thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
