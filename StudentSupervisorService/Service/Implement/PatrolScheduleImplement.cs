using AutoMapper;
using Domain.Entity;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.PatrolScheduleRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.PatrolScheduleResponse;

namespace StudentSupervisorService.Service.Implement
{
    public class PatrolScheduleImplement : PatrolScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatrolScheduleImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<PatrolScheduleResponse>>> GetAllPatrolSchedules(string sortOrder)
        {
            var response = new DataResponse<List<PatrolScheduleResponse>>();
            try
            {

                var pScheduleEntities = await _unitOfWork.PatrolSchedule.GetAllPatrolSchedules();
                if (pScheduleEntities is null || !pScheduleEntities.Any())
                {
                    response.Message = "Danh sách Lịch tuần tra trống!!";
                    response.Success = true;
                    return response;
                }

                pScheduleEntities = sortOrder == "desc"
                    ? pScheduleEntities.OrderByDescending(r => r.ScheduleId).ToList()
                    : pScheduleEntities.OrderBy(r => r.ScheduleId).ToList();

                response.Data = _mapper.Map<List<PatrolScheduleResponse>>(pScheduleEntities);
                response.Message = "Danh sách các lịch tuần tra";
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

        public async Task<DataResponse<PatrolScheduleResponse>> GetPatrolScheduleById(int id)
        {
            var response = new DataResponse<PatrolScheduleResponse>();
            try
            {
                var pScheduleEntity = await _unitOfWork.PatrolSchedule.GetPatrolScheduleById(id);
                if (pScheduleEntity == null)
                {
                    response.Message = "Không tìm thấy Lịch tuần tra!!";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<PatrolScheduleResponse>(pScheduleEntity);
                response.Message = "Đã tìm thấy Lịch tuần tra.";
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

        public async Task<DataResponse<List<PatrolScheduleResponse>>> SearchPatrolSchedules(int? classId, int? supervisorId, int? teacherId, DateTime? from, DateTime? to, string? status ,string sortOrder)
        {
            var response = new DataResponse<List<PatrolScheduleResponse>>();

            try
            {
                var pScheduleEntities = await _unitOfWork.PatrolSchedule.SearchPatrolSchedules(classId, supervisorId, teacherId, from, to, status);
                if (pScheduleEntities is null || pScheduleEntities.Count == 0)
                {
                    response.Message = "Không có Lịch tuần tra phù hợp với tiêu chí tìm kiếm!!";
                    response.Success = true;
                }
                else
                {
                    if (sortOrder == "desc")
                    {
                        pScheduleEntities = pScheduleEntities.OrderByDescending(r => r.ScheduleId).ToList();
                    }
                    else
                    {
                        pScheduleEntities = pScheduleEntities.OrderBy(r => r.ScheduleId).ToList();
                    }
                    response.Data = _mapper.Map<List<PatrolScheduleResponse>>(pScheduleEntities);
                    response.Message = "Danh sách Lịch tuần tra";
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

        public async Task<DataResponse<PatrolScheduleResponse>> CreatePatrolSchedule(PatrolScheduleCreateRequest request)
        {
            var response = new DataResponse<PatrolScheduleResponse>();
            try
            {
                var pScheduleEntity = new PatrolSchedule
                {
                    ClassId = request.ClassId,
                    SupervisorId = request.SupervisorId,
                    TeacherId = request.TeacherId,
                    From = request.From,
                    To = request.To,
                    Status = PatrolScheduleStatusEnums.ONGOING.ToString()
                };

                var created = await _unitOfWork.PatrolSchedule.CreatePatrolSchedule(pScheduleEntity);

                response.Data = _mapper.Map<PatrolScheduleResponse>(created);
                response.Message = "Lịch tuần tra được tạo thành công !!";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo Lịch tuần tra không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<PatrolScheduleResponse>> UpdatePatrolSchedule(PatrolScheduleUpdateRequest request)
        {
            var response = new DataResponse<PatrolScheduleResponse>();
            try
            {
                var existingPatrolSchedule = await _unitOfWork.PatrolSchedule.GetPatrolScheduleById(request.ScheduleId);
                if (existingPatrolSchedule == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Lịch tuần tra !";
                    response.Success = false;
                    return response;
                }

                existingPatrolSchedule.ClassId = request.ClassId ?? existingPatrolSchedule.ClassId;
                existingPatrolSchedule.Supervisor.StudentSupervisorId = request.SupervisorId ?? existingPatrolSchedule.SupervisorId;
                existingPatrolSchedule.TeacherId = request.TeacherId ?? existingPatrolSchedule.TeacherId;
                existingPatrolSchedule.From = request.From ?? existingPatrolSchedule.From;
                existingPatrolSchedule.To = request.To ?? existingPatrolSchedule.To;
                existingPatrolSchedule.Status = request.Status.ToString() ?? existingPatrolSchedule.Status;

                await _unitOfWork.PatrolSchedule.UpdatePatrolSchedule(existingPatrolSchedule);

                response.Data = _mapper.Map<PatrolScheduleResponse>(existingPatrolSchedule);
                response.Message = "Lịch tuần tra được cập nhật thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Cập nhật Lịch tuần tra không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<PatrolScheduleResponse>> DeletePatrolSchedule(int id)
        {
            var response = new DataResponse<PatrolScheduleResponse>();
            try
            {
                var existingPSchedule = await _unitOfWork.PatrolSchedule.GetPatrolScheduleById(id);
                if (existingPSchedule == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy Lịch tuần tra !";
                    response.Success = false;
                    return response;
                }

                if (existingPSchedule.Status == PatrolScheduleStatusEnums.INACTIVE.ToString())
                {
                    response.Data = null;
                    response.Message = "Lịch tuần tra đã bị xóa";
                    response.Success = false;
                    return response;
                }

                await _unitOfWork.PatrolSchedule.DeletePatrolSchedule(id);
                response.Data = "Empty";
                response.Message = "Lịch tuần tra đã được xóa thành công";
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

        public async Task<DataResponse<List<PatrolScheduleResponse>>> GetPatrolSchedulesBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<PatrolScheduleResponse>>();
            try
            {
                var patrolSchedules = await _unitOfWork.PatrolSchedule.GetPatrolSchedulesBySchoolId(schoolId);
                if (patrolSchedules == null || !patrolSchedules.Any())
                {
                    response.Message = "Không tìm thấy Lịch tuần tra nào cho SchoolId được chỉ định!!";
                    response.Success = false;
                }
                else
                {
                    var patrolScheduleDTOs = _mapper.Map<List<PatrolScheduleResponse>>(patrolSchedules);
                    response.Data = patrolScheduleDTOs;
                    response.Message = "Đã tìm thấy Lịch tuần tra";
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
