using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.PackageTypeRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PackageTypeResponse;

namespace StudentSupervisorService.Service.Implement
{
    public class PackageTypeImplement : PackageTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageTypeImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<PackageTypeResponse>> CreatePackageType(PackageTypeRequest request)
        {
            var response = new DataResponse<PackageTypeResponse>();

            try
            {
                var createPackageType = _mapper.Map<PackageType>(request);
                createPackageType.Status = PackageTypeStatusEnums.ACTIVE.ToString();
                _unitOfWork.PackageType.Add(createPackageType);
                _unitOfWork.Save();
                response.Data = _mapper.Map<PackageTypeResponse>(createPackageType);
                response.Message = "Tạo Loại gói thành công !!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<PackageTypeResponse>> DeletePackageType(int id)
        {
            var response = new DataResponse<PackageTypeResponse>();
            try
            {
                var packageType = _unitOfWork.PackageType.GetById(id);
                if (packageType is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Loại gói có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (packageType.Status == PackageTypeStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Loại gói đã bị xóa rồi !!";
                    response.Success = false;
                    return response;
                }

                packageType.Status = PackageTypeStatusEnums.INACTIVE.ToString();
                _unitOfWork.PackageType.Update(packageType);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Loại gói đã được xoá thành công !!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Loại gói không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<PackageTypeResponse>>> GetAllPackageTypes(string sortOrder)
        {
            var response = new DataResponse<List<PackageTypeResponse>>();

            try
            {
                var packageTypes = await _unitOfWork.PackageType.GetAllPackageTypes();
                if (packageTypes is null || !packageTypes.Any())
                {
                    response.Message = "Danh sách Loại gói trống!!";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách PackageType theo yêu cầu
                var packageTypeDTO = _mapper.Map<List<PackageTypeResponse>>(packageTypes);
                if (sortOrder == "desc")
                {
                    packageTypeDTO = packageTypeDTO.OrderByDescending(r => r.PackageTypeId).ToList();
                }
                else
                {
                    packageTypeDTO = packageTypeDTO.OrderBy(r => r.PackageTypeId).ToList();
                }
                response.Data = packageTypeDTO;
                response.Message = "Danh sách các Loại gói";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<PackageTypeResponse>> GetPackageTypeById(int id)
        {
            var response = new DataResponse<PackageTypeResponse>();

            try
            {
                var packageType = await _unitOfWork.PackageType.GetPackageTypeById(id);
                if (packageType is null)
                {
                    throw new Exception("PackageType không tồn tại!!");
                }
                response.Data = _mapper.Map<PackageTypeResponse>(packageType);
                response.Message = $"PackageTypeId {packageType.PackageTypeId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<PackageTypeResponse>>> SearchPackageTypes(string? name, string sortOrder)
        {
            var response = new DataResponse<List<PackageTypeResponse>>();

            try
            {
                var packageTypes = await _unitOfWork.PackageType.SearchPackageTypes(name);
                if (packageTypes is null || packageTypes.Count == 0)
                {
                    response.Message = "Không tìm thấy Loại gói nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var vioTypeDTO = _mapper.Map<List<PackageTypeResponse>>(packageTypes);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        vioTypeDTO = vioTypeDTO.OrderByDescending(p => p.PackageTypeId).ToList();
                    }
                    else
                    {
                        vioTypeDTO = vioTypeDTO.OrderBy(p => p.PackageTypeId).ToList();
                    }

                    response.Data = vioTypeDTO;
                    response.Message = "Tìm thấy Loại gói";
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

        public async Task<DataResponse<PackageTypeResponse>> UpdatePackageType(int id, PackageTypeRequest request)
        {
            var response = new DataResponse<PackageTypeResponse>();

            try
            {
                var packageType = _unitOfWork.PackageType.GetById(id);
                if (packageType is null)
                {
                    response.Message = "Không tìm được Loại gói";
                    response.Success = false;
                    return response;
                }
                packageType.Name = request.Name;
                packageType.Description = request.Description;

                _unitOfWork.PackageType.Update(packageType);
                _unitOfWork.Save();

                response.Data = _mapper.Map<PackageTypeResponse>(packageType);
                response.Success = true;
                response.Message = "Cập nhật Loại gói thành công.";
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
