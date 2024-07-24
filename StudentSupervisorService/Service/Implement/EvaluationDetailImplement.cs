using AutoMapper;
using Azure.Core;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.EvaluationDetailRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.EvaluationDetailResponse;

namespace StudentSupervisorService.Service.Implement
{
    public class EvaluationDetailImplement : EvaluationDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EvaluationDetailImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<EvaluationDetailResponse>>> GetAllEvaluationDetails(string sortOrder)
        {
            var response = new DataResponse<List<EvaluationDetailResponse>>();
            try
            {

                var evaluationDetailEntities = await _unitOfWork.EvaluationDetail.GetAllEvaluationDetails();
                if (evaluationDetailEntities is null || !evaluationDetailEntities.Any())
                {
                    response.Message = "Danh sách Đánh giá Chi tiết trống";
                    response.Success = true;
                    return response;
                }

                evaluationDetailEntities = sortOrder == "desc"
                    ? evaluationDetailEntities.OrderByDescending(r => r.EvaluationDetailId).ToList()
                    : evaluationDetailEntities.OrderBy(r => r.EvaluationDetailId).ToList();

                response.Data = _mapper.Map<List<EvaluationDetailResponse>>(evaluationDetailEntities);
                response.Message = "Danh sách các đánh giá Chi tiết";
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

        public async Task<DataResponse<EvaluationDetailResponse>> GetEvaluationDetailById(int id)
        {
            var response = new DataResponse<EvaluationDetailResponse>();
            try
            {
                var evaluationDetailEntity = await _unitOfWork.EvaluationDetail.GetEvaluationDetailById(id);
                if (evaluationDetailEntity == null)
                {
                    response.Message = "Không tìm thấy Đánh giá Chi tiết";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<EvaluationDetailResponse>(evaluationDetailEntity);
                response.Message = "Tìm thấy Đánh giá Chi tiết";
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

        public async Task<DataResponse<List<EvaluationDetailResponse>>> SearchEvaluationDetails(int? classId, int? evaluationId, string? status, string sortOrder)
        {
            var response = new DataResponse<List<EvaluationDetailResponse>>();

            try
            {
                var evaluationDetailEntities = await _unitOfWork.EvaluationDetail.SearchEvaluationDetails(classId, evaluationId, status);
                if (evaluationDetailEntities is null || evaluationDetailEntities.Count == 0)
                {
                    response.Message = "Không có Đánh giá Chi tiết nào phù hợp với tiêu chí tìm kiếm";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        evaluationDetailEntities = evaluationDetailEntities.OrderByDescending(r => r.EvaluationDetailId).ToList();
                    }
                    else
                    {
                        evaluationDetailEntities = evaluationDetailEntities.OrderBy(r => r.EvaluationDetailId).ToList();
                    }
                    response.Data = _mapper.Map<List<EvaluationDetailResponse>>(evaluationDetailEntities);
                    response.Message = "Danh sách Đánh giá Chi tiết";
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
        public async Task<DataResponse<EvaluationDetailResponse>> CreateEvaluationDetail(EvaluationDetailCreateRequest request)
        {
            var response = new DataResponse<EvaluationDetailResponse>();
            try
            {
                var evaluationDetailEntity = new EvaluationDetail
                {
                    ClassId = request.ClassId,
                    EvaluationId = request.EvaluationId,
                    Status = EvaluationDetailStatusEnums.ACTIVE.ToString()
                };

                var created = await _unitOfWork.EvaluationDetail.CreateEvaluationDetail(evaluationDetailEntity);

                response.Data = _mapper.Map<EvaluationDetailResponse>(created);
                response.Message = "Đánh giá chi tiết được tạo thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Đánh giá Chi tiết không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<EvaluationDetailResponse>> UpdateEvaluationDetail(EvaluationDetailUpdateRequest request)
        {
            var response = new DataResponse<EvaluationDetailResponse>();
            try
            {
                var existingEvaluationDetail = await _unitOfWork.EvaluationDetail.GetEvaluationDetailById(request.EvaluationDetailId);
                if (existingEvaluationDetail == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Đánh giá Chi tiết !!";
                    response.Success = false;
                    return response;
                }

                existingEvaluationDetail.ClassId = request.ClassId ?? existingEvaluationDetail.ClassId;
                existingEvaluationDetail.EvaluationId = request.EvaluationId ?? existingEvaluationDetail.EvaluationId;
                existingEvaluationDetail.Status = request.Status.ToString() ?? existingEvaluationDetail.Status;

                await _unitOfWork.EvaluationDetail.UpdateEvaluationDetail(existingEvaluationDetail);

                response.Data = _mapper.Map<EvaluationDetailResponse>(existingEvaluationDetail);
                response.Message = "Đánh giá Chi tiết được cập nhật thành công!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Cập nhật Đánh giá Chi tiết không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<EvaluationDetailResponse>> DeleteEvaluationDetail(int id)
        {
            var response = new DataResponse<EvaluationDetailResponse>();
            try
            {
                var existingEvaluationDetail = await _unitOfWork.EvaluationDetail.GetEvaluationDetailById(id);
                if (existingEvaluationDetail == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Đánh giá Chi tiết !!";
                    response.Success = false;
                    return response;
                }

                if (existingEvaluationDetail.Status == EvaluationDetailStatusEnums.INACTIVE.ToString())
                {
                    response.Data = null;
                    response.Message = "Đánh giá Chi tiết đã bị xóa !!";
                    response.Success = false;
                    return response;
                }

                await _unitOfWork.EvaluationDetail.DeleteEvaluationDetail(id);
                response.Data = "Empty";
                response.Message = "Đánh giá Chi tiết đã được xóa thành công";
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

        public async Task<DataResponse<List<EvaluationDetailResponse>>> GetEvaluationDetailsBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<EvaluationDetailResponse>>();
            try
            {
                var evaluationDetails = await _unitOfWork.EvaluationDetail.GetEvaluationDetailsBySchoolId(schoolId);
                if (evaluationDetails == null || !evaluationDetails.Any())
                {
                    response.Message = "Không tìm thấy Đánh giá Chi tiết nào cho SchoolId được chỉ định";
                    response.Success = false;
                }
                else
                {
                    var evaluationDetailDTOs = _mapper.Map<List<EvaluationDetailResponse>>(evaluationDetails);
                    response.Data = evaluationDetailDTOs;
                    response.Message = "Đánh giá Chi tiết được tìm thấy";
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
