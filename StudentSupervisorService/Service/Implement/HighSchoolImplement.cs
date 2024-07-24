using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.HighSchoolRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.HighschoolResponse;
using StudentSupervisorService.Models.Response.SchoolYearResponse;
using StudentSupervisorService.Models.Response.ViolationGroupResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSupervisorService.Service.Implement
{
    public class HighSchoolImplement : HighSchoolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HighSchoolImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<ResponseOfHighSchool>> CreateHighSchool(RequestOfHighSchool request)
        {
            var response = new DataResponse<ResponseOfHighSchool>();

            try
            {

                // Kiểm tra xem code, name, phone đã tồn tại hay chưa
                var existingHighSchool = _unitOfWork.HighSchool.Find(
                                        h => h.Code == request.Code ||
                                        h.Name == request.Name ||
                                        h.Phone == request.Phone).FirstOrDefault();
                if (existingHighSchool != null)
                {
                    response.Data = "Empty";
                    response.Message = "Trường trung học phổ thông đã tồn tại !!";
                    response.Success = false;
                    return response;
                }

                var createHighSchool = _mapper.Map<HighSchool>(request);
                var registerSchool = new RegisteredSchool
                {
                    SchoolId = createHighSchool.SchoolId,
                    RegisteredDate = DateTime.Now,
                    Status = RegisteredSchoolStatusEnums.ACTIVE.ToString()
                };
                createHighSchool.RegisteredSchools.Add(registerSchool);
                createHighSchool.Status = HighSchoolStatusEnums.ACTIVE.ToString();

                _unitOfWork.HighSchool.Add(createHighSchool);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfHighSchool>(createHighSchool);
                response.Message = "Tạo Trường trung học phổ thông thành công !!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfHighSchool>> DeleteHighSchool(int id)
        {
            var response = new DataResponse<ResponseOfHighSchool>();
            try
            {
                var highSchool = _unitOfWork.HighSchool.GetById(id);
                if (highSchool is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không thể tìm thấy Trường trung học phổ thông có ID: " + id;
                    response.Success = false;
                    return response;
                }

                if (highSchool.Status == HighSchoolStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Trường trung học phổ thông đã bị xóa !!";
                    response.Success = false;
                    return response;
                }

                highSchool.Status = HighSchoolStatusEnums.INACTIVE.ToString();
                _unitOfWork.HighSchool.Update(highSchool);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Trường trung học phổ thông đã được xóa thành công !!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Xóa Trường trung học thất bại: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ResponseOfHighSchool>>> GetAllHighSchools(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfHighSchool>>();

            try
            {
                var highSchool = await _unitOfWork.HighSchool.GetAllHighSchools();
                if (highSchool is null || !highSchool.Any())
                {
                    response.Message = "Danh sách Trường trung học phổ thông trống !!";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách Violation Group theo yêu cầu
                var highSchoolDTO = _mapper.Map<List<ResponseOfHighSchool>>(highSchool);
                if (sortOrder == "desc")
                {
                    highSchoolDTO = highSchoolDTO.OrderByDescending(r => r.SchoolId).ToList();
                }
                else
                {
                    highSchoolDTO = highSchoolDTO.OrderBy(r => r.SchoolId).ToList();
                }
                response.Data = highSchoolDTO;
                response.Message = "Danh sách các Trường trung học phổ thông";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfHighSchool>> GetHighSchoolById(int id)
        {
            var response = new DataResponse<ResponseOfHighSchool>();

            try
            {
                var highSchool = await _unitOfWork.HighSchool.GetHighSchoolById(id);
                if (highSchool is null)
                {
                    throw new Exception("Trường Trung học phổ thông không tồn tại !!");
                }
                response.Data = _mapper.Map<ResponseOfHighSchool>(highSchool);
                response.Message = $"SchoolId {highSchool.SchoolId}";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message;
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfHighSchool>>> SearchHighSchools(string? code, string? name, string? city, string? address, string? phone, string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfHighSchool>>();

            try
            {
                var highSchools = await _unitOfWork.HighSchool.SearchHighSchools(code, name, city, address, phone);
                if (highSchools is null || highSchools.Count == 0)
                {
                    response.Message = "Không tìm thấy tTrường trung học phổ thông nào phù hợp với tiêu chí !!";
                    response.Success = true;
                }
                else
                {
                    var highSchoolDTO = _mapper.Map<List<ResponseOfHighSchool>>(highSchools);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        highSchoolDTO = highSchoolDTO.OrderByDescending(p => p.SchoolId).ToList();
                    }
                    else
                    {
                        highSchoolDTO = highSchoolDTO.OrderBy(p => p.SchoolId).ToList();
                    }

                    response.Data = highSchoolDTO;
                    response.Message = "Tìm thấy Trường trung học phổ thông!";
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

        public async Task<DataResponse<ResponseOfHighSchool>> UpdateHighSchool(int id, RequestOfHighSchool request)
        {
            var response = new DataResponse<ResponseOfHighSchool>();

            try
            {
                var highSchool = _unitOfWork.HighSchool.GetById(id);
                if (highSchool is null)
                {
                    response.Message = "Không thể tìm thấy Trường trung học phổ thông!!";
                    response.Success = false;
                    return response;
                }

                var isExistCode = _unitOfWork.HighSchool.Find(h => h.Code == request.Code && h.SchoolId != id).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã trường đã được sử dụng";
                    response.Success = false;
                    return response;
                }

                highSchool.Code = request.Code;
                highSchool.Name = request.Name;
                highSchool.City = request.City;
                highSchool.Address = request.Address;
                highSchool.Phone = request.Phone;
                highSchool.WebUrl = request.WebUrl;
                _unitOfWork.HighSchool.Update(highSchool);
                _unitOfWork.Save();
                response.Data = _mapper.Map<ResponseOfHighSchool>(highSchool);
                response.Success = true;
                response.Message = "Cập nhật thành công!!";
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
