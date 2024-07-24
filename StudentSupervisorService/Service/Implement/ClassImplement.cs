using AutoMapper;
using Domain.Entity;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.ClassRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ClassResponse;


namespace StudentSupervisorService.Service.Implement
{
    public class ClassImplement : ClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassImplement(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DataResponse<List<ClassResponse>>> GetAllClasses(string sortOrder)
        {
            var response = new DataResponse<List<ClassResponse>>();
            try
            {
                var classEntities = await _unitOfWork.Class.GetAllClasses();
                if (classEntities is null || !classEntities.Any())
                {
                    response.Message = "Danh sách lớp học trống!";
                    response.Success = true;
                    return response;
                }

                classEntities = sortOrder == "desc"
                    ? classEntities.OrderByDescending(r => r.ClassId).ToList()
                    : classEntities.OrderBy(r => r.ClassId).ToList();

                response.Data = _mapper.Map<List<ClassResponse>>(classEntities);
                response.Message = "Danh sách các lớp học";
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

        public async Task<DataResponse<ClassResponse>> GetClassById(int id)
        {
            var response = new DataResponse<ClassResponse>();
            try
            {
                var classEntity = await _unitOfWork.Class.GetClassById(id);
                if (classEntity == null)
                {
                    response.Message = "Không tìm thấy lớp học !!";
                    response.Success = false;
                    return response;
                }

                response.Data = _mapper.Map<ClassResponse>(classEntity);
                response.Message = "Đã tìm thấy lớp học";
                response.Success = true;
            } catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ClassResponse>>> SearchClasses(int? schoolYearId, int? classGroupId, string? code, int? grade, string? name, int? totalPoint, string sortOrder)
        {
            var response = new DataResponse<List<ClassResponse>>();

            try
            {
                var classEntities = await _unitOfWork.Class.SearchClasses(schoolYearId, classGroupId, code, grade, name, totalPoint);
                if (classEntities is null || classEntities.Count == 0)
                {
                    response.Message = "Không có lớp nào phù hợp với tiêu chí tìm kiếm !!";
                    response.Success = true;
                } else
                {
                    if (sortOrder == "desc")
                    {
                        classEntities = classEntities.OrderByDescending(r => r.ClassId).ToList();
                    } else
                    {
                        classEntities = classEntities.OrderBy(r => r.ClassId).ToList();
                    }
                    response.Data = _mapper.Map<List<ClassResponse>>(classEntities);
                    response.Message = "Danh sách lớp học";
                    response.Success = true;
                }
            } catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ClassResponse>> CreateClass(ClassCreateRequest request)
        {
            var response = new DataResponse<ClassResponse>();
            try
            {
                var isExistCode = _unitOfWork.Class.Find(s => s.Code == request.Code).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã lớp đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                var isExistName = _unitOfWork.Class.Find(s => s.Name == request.Name).FirstOrDefault();
                if (isExistName != null)
                {
                    response.Message = "Tên lớp đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                var classEntity = new Class
                {
                    SchoolYearId = request.SchoolYearId,
                    ClassGroupId = request.ClassGroupId,
                    TeacherId = request.TeacherId,
                    Code = request.Code,
                    Grade = request.Grade,
                    Name = request.Name,
                    TotalPoint = request.TotalPoint
                };

                var created = await _unitOfWork.Class.CreateClass(classEntity);

                response.Data = _mapper.Map<ClassResponse>(created);
                response.Message = "Lớp được tạo thành công";
                response.Success = true;
            } catch (Exception ex)
            {
                response.Message = "Tạo lớp không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ClassResponse>> UpdateClass(ClassUpdateRequest request)
        {
            var response = new DataResponse<ClassResponse>();
            try
            {
                var existingClass = await _unitOfWork.Class.GetClassById(request.ClassId);
                if (existingClass == null)
                {
                    response.Data = "Trống";
                    response.Message = "Không tìm thấy lớp học !!";
                    response.Success = false;
                    return response;
                }

                var isExistCode = _unitOfWork.Class.Find(s => s.Code == request.Code && s.ClassId != request.ClassId).FirstOrDefault();
                if (isExistCode != null)
                {
                    response.Message = "Mã lớp đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                var isExistName = _unitOfWork.Class.Find(s => s.Name == request.Name && s.ClassId != request.ClassId).FirstOrDefault();
                if (isExistName != null)
                {
                    response.Message = "Tên lớp đã được sử dụng !!";
                    response.Success = false;
                    return response;
                }

                existingClass.SchoolYearId = request.SchoolYearId ?? existingClass.SchoolYearId;
                existingClass.ClassGroupId = request.ClassGroupId ?? existingClass.ClassGroupId;
                existingClass.TeacherId = request.TeacherId ?? existingClass.TeacherId;
                existingClass.Code = request.Code ?? existingClass.Code;
                existingClass.Grade = request.Grade ?? existingClass.Grade;
                existingClass.Name = request.Name ?? existingClass.Name;
                existingClass.TotalPoint = request.TotalPoint ?? existingClass.TotalPoint;

                await _unitOfWork.Class.UpdateClass(existingClass);

                response.Data = _mapper.Map<ClassResponse>(existingClass);
                response.Message = "Lớp được cập nhật thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Cập nhật lớp không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ClassResponse>> DeleteClass(int id)
        {
            var response = new DataResponse<ClassResponse>();
            try
            {
                var existingClass = await _unitOfWork.Class.GetClassById(id);
                if (existingClass == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy lớp học !!";
                    response.Success = false;
                    return response;
                }
                await _unitOfWork.Class.DeleteClass(id);
                response.Data = "Empty";
                response.Message = "Lớp đã được xóa thành công !!";
                response.Success = true;
            } catch (Exception ex)
            {
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ClassResponse>>> GetClassesBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ClassResponse>>();
            try
            {
                var classes = await _unitOfWork.Class.GetClasssBySchoolId(schoolId);
                if (classes == null || !classes.Any())
                {
                    response.Message = "Không tìm thấy Lớp nào cho SchoolId được chỉ định";
                    response.Success = false;
                }
                else
                {
                    var classDTOs = _mapper.Map<List<ClassResponse>>(classes);
                    response.Data = classDTOs;
                    response.Message = "Đã tìm thấy lớp học";
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
