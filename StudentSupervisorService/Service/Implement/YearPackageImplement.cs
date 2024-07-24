using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.YearPackageRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.YearPackageResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentSupervisorService.Service.Implement
{
    public class YearPackageImplement : YearPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public YearPackageImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<DataResponse<ResponseOfYearPackage>> CreateYearPackage(RequestOfYearPackage request)
        {
            var response = new DataResponse<ResponseOfYearPackage>();

            try
            {
                var createYearPackage = _mapper.Map<YearPackage>(request);
                createYearPackage.Status = YearPackageStatusEnums.VALID.ToString();
                _unitOfWork.YearPackage.Add(createYearPackage);
                _unitOfWork.Save();
                response.Data = _mapper.Map<ResponseOfYearPackage>(createYearPackage);
                response.Message = "Tạo Gói năm thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Gói năm thất bại.: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfYearPackage>> DeleteYearPackage(int id)
        {
            var response = new DataResponse<ResponseOfYearPackage>();
            try
            {
                var yearPackage = _unitOfWork.YearPackage.GetById(id);
                if (yearPackage is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Gói năm có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (yearPackage.Status == YearPackageStatusEnums.EXPIRED.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Gói năm đã bị xóa";
                    response.Success = false;
                    return response;
                }

                yearPackage.Status = YearPackageStatusEnums.EXPIRED.ToString();
                _unitOfWork.YearPackage.Update(yearPackage);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Gói năm đã được xóa thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Gói năm không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ResponseOfYearPackage>>> GetAllYearPackages(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfYearPackage>>();

            try
            {
                var yearPackages = await _unitOfWork.YearPackage.GetAllYearPackages();
                if (yearPackages is null || !yearPackages.Any())
                {
                    response.Message = "Danh sách Gói năm trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách sản phẩm theo yêu cầu
                var yearPackageDTO = _mapper.Map<List<ResponseOfYearPackage>>(yearPackages);
                if (sortOrder == "desc")
                {
                    yearPackageDTO = yearPackageDTO.OrderByDescending(r => r.YearPackageId).ToList();
                }
                else
                {
                    yearPackageDTO = yearPackageDTO.OrderBy(r => r.YearPackageId).ToList();
                }
                response.Data = yearPackageDTO;
                response.Message = "Danh sách các Gói năm";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfYearPackage>> GetYearPackageById(int id)
        {
            var response = new DataResponse<ResponseOfYearPackage>();

            try
            {
                var yearPackage = await _unitOfWork.YearPackage.GetYearPackageById(id);
                if (yearPackage is null)
                {
                    throw new Exception("Gói Năm không tồn tại");
                }
                response.Data = _mapper.Map<ResponseOfYearPackage>(yearPackage);
                response.Message = $"YearPackageId {yearPackage.YearPackageId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfYearPackage>>> GetYearPackagesBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ResponseOfYearPackage>>();
            try
            {
                var yearPackages = await _unitOfWork.YearPackage.GetYearPackagesBySchoolId(schoolId);
                if (yearPackages == null || !yearPackages.Any())
                {
                    response.Message = "Không tìm thấy Gói Năm nào cho SchoolId được chỉ định";
                    response.Success = false;
                }
                else
                {
                    var yearPackageDTOs = _mapper.Map<List<ResponseOfYearPackage>>(yearPackages);
                    response.Data = yearPackageDTOs;
                    response.Message = "Tìm thấy Gói năm";
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

        public async Task<DataResponse<List<ResponseOfYearPackage>>> SearchYearPackages(int? schoolYearId, int? packageId, int? minNumberOfStudent, int? maxNumberOfStudent, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfYearPackage>>();

            try
            {
                var yearPackages = await _unitOfWork.YearPackage.SearchYearPackages(schoolYearId, packageId, minNumberOfStudent, maxNumberOfStudent);
                if (yearPackages is null || yearPackages.Count == 0)
                {
                    response.Message = "Không tìm thấy Gói năm nào phù hợp với tiêu chí";
                    response.Success = true;
                }
                else
                {
                    var yearPackageDTO = _mapper.Map<List<ResponseOfYearPackage>>(yearPackages);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        yearPackageDTO = yearPackageDTO.OrderByDescending(p => p.YearPackageId).ToList();
                    }
                    else
                    {
                        yearPackageDTO = yearPackageDTO.OrderBy(p => p.YearPackageId).ToList();
                    }

                    response.Data = yearPackageDTO;
                    response.Message = "Tìm thấy Gói năm";
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

        public async Task<DataResponse<ResponseOfYearPackage>> UpdateYearPackage(int id, RequestOfYearPackage request)
        {
            var response = new DataResponse<ResponseOfYearPackage>();

            try
            {
                var yearPackage = _unitOfWork.YearPackage.GetById(id);
                if (yearPackage is null)
                {
                    response.Message = "Không thể tìm thấy Gói năm";
                    response.Success = false;
                    return response;
                }

                yearPackage.SchoolYearId = request.SchoolYearId;
                yearPackage.PackageId = request.PackageId;
                yearPackage.NumberOfStudent = request.NumberOfStudent;
       

                _unitOfWork.YearPackage.Update(yearPackage);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfYearPackage>(yearPackage);
                response.Success = true;
                response.Message = "Cập nhật Gói năm thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Cập nhật Gói năm thất bại.: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
