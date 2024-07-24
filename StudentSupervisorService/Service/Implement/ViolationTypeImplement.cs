using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.ViolationTypeRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.HighschoolResponse;
using StudentSupervisorService.Models.Response.ViolationResponse;
using StudentSupervisorService.Models.Response.ViolationTypeResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentSupervisorService.Service.Implement
{
    public class ViolationTypeImplement : ViolationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ViolationTypeImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<ResponseOfVioType>> CreateVioType(RequestOfVioType request)
        {
            var response = new DataResponse<ResponseOfVioType>();

            try
            {
                var createVioType = _mapper.Map<ViolationType>(request);
                createVioType.Status = ViolationTypeStatusEnums.ACTIVE.ToString();
                _unitOfWork.ViolationType.Add(createVioType);
                _unitOfWork.Save();
                var created = await _unitOfWork.ViolationType.GetVioTypeById(createVioType.ViolationTypeId);
                response.Data = _mapper.Map<ResponseOfVioType>(created);
                response.Message = "Tạo Loại vi phạm thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Loại vi phạm thất bại.: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfVioType>> DeleteVioType(int id)
        {
            var response = new DataResponse<ResponseOfVioType>();
            try
            {
                var vioType = _unitOfWork.ViolationType.GetById(id);
                if (vioType is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Loại vi phạm có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (vioType.Status == ViolationTypeStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Loại vi phạm đã bị xóa.";
                    response.Success = false;
                    return response;
                }

                vioType.Status = ViolationTypeStatusEnums.INACTIVE.ToString();
                _unitOfWork.ViolationType.Update(vioType);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Loại vi phạm đã được xóa thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Loại vi phạm không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ResponseOfVioType>>> GetAllVioTypes(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfVioType>>();

            try
            {
                var vioTypes = await _unitOfWork.ViolationType.GetAllVioTypes();
                if (vioTypes is null || !vioTypes.Any())
                {
                    response.Message = "Danh sách Loại vi phạm trống!!";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách sản phẩm theo yêu cầu
                var vioTypeDTO = _mapper.Map<List<ResponseOfVioType>>(vioTypes);
                if (sortOrder == "desc")
                {
                    vioTypeDTO = vioTypeDTO.OrderByDescending(r => r.ViolationTypeId).ToList();
                }
                else
                {
                    vioTypeDTO = vioTypeDTO.OrderBy(r => r.ViolationTypeId).ToList();
                }
                response.Data = vioTypeDTO;
                response.Message = "Liệt kê các loại vi phạm";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfVioType>>> GetViolationTypesBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ResponseOfVioType>>();
            try
            {
                var violationTypes = await _unitOfWork.ViolationType.GetViolationTypesBySchoolId(schoolId);
                if (violationTypes == null || !violationTypes.Any())
                {
                    response.Message = "Không tìm thấy Loại vi phạm nào cho SchoolId được chỉ định!!";
                    response.Success = false;
                }
                else
                {
                    var violationTypeDTOs = _mapper.Map<List<ResponseOfVioType>>(violationTypes);
                    response.Data = violationTypeDTOs;
                    response.Message = "Đã tìm thấy các loại vi phạm";
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

        public async Task<DataResponse<ResponseOfVioType>> GetVioTypeById(int id)
        {
            var response = new DataResponse<ResponseOfVioType>();

            try
            {
                var vioType = await _unitOfWork.ViolationType.GetVioTypeById(id);
                if (vioType is null)
                {
                    throw new Exception("Loại vi phạm không tồn tại!!");
                }
                response.Data = _mapper.Map<ResponseOfVioType>(vioType);
                response.Message = $"ViolationTypeId {vioType.ViolationTypeId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfVioType>>> SearchVioTypes(int? vioGroupId, string? name, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfVioType>>();

            try
            {
                var violations = await _unitOfWork.ViolationType.SearchVioTypes(vioGroupId, name);
                if (violations is null || violations.Count == 0)
                {
                    response.Message = "Không tìm thấy Loại vi phạm nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var vioTypeDTO = _mapper.Map<List<ResponseOfVioType>>(violations);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        vioTypeDTO = vioTypeDTO.OrderByDescending(p => p.ViolationTypeId).ToList();
                    }
                    else
                    {
                        vioTypeDTO = vioTypeDTO.OrderBy(p => p.ViolationTypeId).ToList();
                    }

                    response.Data = vioTypeDTO;
                    response.Message = "Đã tìm thấy các loại vi phạm";
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

        public async Task<DataResponse<ResponseOfVioType>> UpdateVioType(int id, RequestOfVioType request)
        {
            var response = new DataResponse<ResponseOfVioType>();

            try
            {
                var vioType = await _unitOfWork.ViolationType.GetVioTypeById(id);
                if (vioType is null)
                {
                    response.Message = "Không thể tìm thấy Loại vi phạm";
                    response.Success = false;
                    return response;
                }
                vioType.ViolationGroupId = request.ViolationGroupId;
                vioType.Name = request.VioTypeName;
                vioType.Description = request.Description;

                _unitOfWork.ViolationType.Update(vioType);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfVioType>(vioType);
                response.Success = true;
                response.Message = "Cập nhật thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật thất bại.: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
