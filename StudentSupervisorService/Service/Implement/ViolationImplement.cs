using AutoMapper;
using Azure.Core;
using Domain.Entity;
using Domain.Entity.DTO;
using Domain.Enums.Status;
using Infrastructures.Interfaces.IUnitOfWork;
using StudentSupervisorService.Models.Request.ViolationRequest;
using StudentSupervisorService.Models.Response;
using StudentSupervisorService.Models.Response.ViolationResponse;
using System.Net;
using static System.Net.Mime.MediaTypeNames;


namespace StudentSupervisorService.Service.Implement
{
    public class ViolationImplement : ViolationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ImageUrlService _imageUrlService;
        public ViolationImplement(IUnitOfWork unitOfWork, IMapper mapper, ImageUrlService imageUrlService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageUrlService = imageUrlService;
        }
        public async Task<DataResponse<List<ResponseOfViolation>>> GetAllViolations(string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetAllViolations();
                if (violations is null || !violations.Any())
                {
                    response.Data = "Empty";
                    response.Message = "Danh sách vi phạm trống";
                    response.Success = true;
                    return response;
                }
                // Sắp xếp danh sách Violation theo yêu cầu
                var vioDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                if (sortOrder == "desc")
                {
                    vioDTO = vioDTO.OrderByDescending(r => r.ViolationId).ToList();
                }
                else
                {
                    vioDTO = vioDTO.OrderBy(r => r.ViolationId).ToList();
                }
                response.Data = vioDTO;
                response.Message = "Danh sách các vi phạm";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> GetViolationById(int id)
        {
            var response = new DataResponse<ResponseOfViolation>();

            try
            {
                var violation = await _unitOfWork.Violation.GetViolationById(id);
                if (violation is null)
                {
                    response.Data = "Empty";
                    response.Message = "Vi phạm không tồn tại";
                    response.Success = false;
                    throw new Exception("Vi phạm không tồn tại");
                }
                response.Data = _mapper.Map<ResponseOfViolation>(violation);
                response.Message = $"ViolationId {violation.ViolationId}";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> SearchViolations(
                int? classId,
                int? violationTypeId,
                int? studentInClassId,
                int? teacherId,
                string? name,
                string? description,
                DateTime? date,
                string? status, 
                string sortOrder)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.SearchViolations(classId, violationTypeId, studentInClassId, teacherId, name, description, date, status);
                if (violations is null || violations.Count == 0)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy vi phạm nào phù hợp với tiêu chí!!";
                    response.Success = true;
                }
                else
                {
                    var violationDTO = _mapper.Map<List<ResponseOfViolation>>(violations);

                    // Thực hiện sắp xếp
                    if (sortOrder == "desc")
                    {
                        violationDTO = violationDTO.OrderByDescending(p => p.ViolationId).ToList();
                    }
                    else
                    {
                        violationDTO = violationDTO.OrderBy(p => p.ViolationId).ToList();
                    }

                    response.Data = violationDTO;
                    response.Message = "Đã tìm thấy vi phạm";
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

        public async Task<DataResponse<ResponseOfViolation>> CreateViolationForStudentSupervisor(RequestOfCreateViolation request)
        {
            var response = new DataResponse<ResponseOfViolation>();
            try
            {
                // Mapping request to Violation entity
                var violationEntity = new Violation
                {
                    ClassId = request.ClassId,
                    ViolationTypeId = request.ViolationTypeId,
                    StudentInClassId = request.StudentInClassId,
                    TeacherId = request.TeacherId,
                    Name = request.ViolationName,
                    Description = request.Description,
                    Date = request.Date,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Status = ViolationStatusEnums.PENDING.ToString()
                };

                if (request.Images != null)
                {
                    var first2Images = request.Images.Take(2).ToList(); // just take first 2 images to upload
                    foreach (var image in first2Images)
                    {
                        // Upload image to cloudinary
                        var uploadResult = await _imageUrlService.UploadImage(image);
                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            violationEntity.ImageUrls.Add(new ImageUrl
                            {
                                ViolationId = violationEntity.ViolationId,
                                Url = uploadResult.SecureUrl.AbsoluteUri,
                                Name = uploadResult.PublicId,
                                Description = "Hình ảnh của " + violationEntity.ViolationId + " vi phạm"
                            });
                        }
                    }
                }
                // Save Violation to database
                await _unitOfWork.Violation.CreateViolation(violationEntity);

                response.Data = _mapper.Map<ResponseOfViolation>(violationEntity);
                response.Message = "Tạo vi phạm thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> CreateViolationForSupervisor(RequestOfCreateViolation request)
        {
            var response = new DataResponse<ResponseOfViolation>();
            try
            {
                // Mapping request to Violation entity
                var violationEntity = new Violation
                {
                    ClassId = request.ClassId,
                    ViolationTypeId = request.ViolationTypeId,
                    StudentInClassId = request.StudentInClassId,
                    TeacherId = request.TeacherId,
                    Name = request.ViolationName,
                    Description = request.Description,
                    Date = request.Date,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Status = ViolationStatusEnums.APPROVED.ToString()
                };

                if (request.Images != null)
                {
                    var first2Images = request.Images.Take(2).ToList(); // just take first 2 images to upload
                    foreach (var image in first2Images)
                    {
                        // Upload image to cloudinary
                        var uploadResult = await _imageUrlService.UploadImage(image);
                        if (uploadResult.StatusCode == HttpStatusCode.OK)
                        {
                            violationEntity.ImageUrls.Add(new ImageUrl
                            {
                                ViolationId = violationEntity.ViolationId,
                                Url = uploadResult.SecureUrl.AbsoluteUri,
                                Name = uploadResult.PublicId,
                                Description = "Hình ảnh của " + violationEntity.ViolationId + " vi phạm"
                            });
                        }
                    }
                }
                // Save Violation to database
                await _unitOfWork.Violation.CreateViolation(violationEntity);

                // Tạo Discipline cho Violation tương ứng
                var disciplineEntity = new Discipline
                {
                    ViolationId = violationEntity.ViolationId,
                    PennaltyId = null, // Set a default PennaltyId or fetch based on some logic
                    Description = violationEntity.Name,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(7), // Set default EndDate
                    Status = DisciplineStatusEnums.PENDING.ToString()
                };
 
                await _unitOfWork.Discipline.CreateDiscipline(disciplineEntity);

                // Increase NumberOfViolation in StudentInClass
                var studentInClass = _unitOfWork.StudentInClass.GetById(violationEntity.StudentInClassId.Value);
                if (studentInClass != null)
                {
                    studentInClass.NumberOfViolation = (studentInClass.NumberOfViolation ?? 0) + 1;
                    _unitOfWork.StudentInClass.Update(studentInClass);
                    _unitOfWork.Save();
                }

                response.Data = _mapper.Map<ResponseOfViolation>(violationEntity);
                response.Message = "Tạo vi phạm thành công.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Message = "Tạo vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> UpdateViolation(int id, RequestOfUpdateViolation request)
        {
            var response = new DataResponse<ResponseOfViolation>();
            try
            {
                var violation = _unitOfWork.Violation.GetById(id);
                if (violation == null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy vi phạm!!";
                    response.Success = false;
                    return response;
                }

                violation.ClassId = request.ClassId;
                violation.ViolationTypeId = request.ViolationTypeId;
                violation.StudentInClassId = request.StudentInClassId;
                violation.TeacherId = request.TeacherId;
                violation.Name = request.ViolationName;
                violation.Description = request.Description;
                violation.Date = request.Date;
                violation.UpdatedAt = DateTime.Now;

                _unitOfWork.Violation.Update(violation);
                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfViolation>(violation);
                response.Message = "Đã cập nhật vi phạm thành công";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Vi phạm cập nhật không thành công: " + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> DeleteViolation(int id)
        {
            var response = new DataResponse<ResponseOfViolation>();
            try
            {
                var violation = _unitOfWork.Violation.GetById(id);
                if (violation is null)
                {
                    response.Data = "Empty";
                    response.Message = "Không tìm thấy vi phạm!!";
                    response.Success = false;
                    return response;
                }

                if (violation.Status == ViolationStatusEnums.INACTIVE.ToString())
                {
                    response.Data = "Empty";
                    response.Message = "Vi phạm đã bị xóa.";
                    response.Success = false;
                    return response;
                }

                violation.Status = ViolationStatusEnums.INACTIVE.ToString();
                _unitOfWork.Violation.Update(violation);
                _unitOfWork.Save();

                response.Data = "Empty";
                response.Message = "Xóa vi phạm thành công.";
                response.Success = true;
            } catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Xóa vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> ApproveViolation(int violationId)
        {
            var response = new DataResponse<ResponseOfViolation>();

            try
            {
                var violation = await _unitOfWork.Violation.GetViolationById(violationId);
                if (violation == null)
                {
                    response.Message = "Không thể tìm thấy vi phạm!!";
                    response.Success = false;
                    return response;
                }

                if (violation.Status == ViolationStatusEnums.APPROVED.ToString())
                {
                    response.Message = "Vi phạm đã ở trạng thái Approved.";
                    response.Success = false;
                    return response;
                }

                // Kiểm tra xem Discipline tương ứng với Vioation đó đã được tạo chưa
                var discipline = await _unitOfWork.Discipline.GetDisciplineByViolationId(violation.ViolationId);

                if (violation.Status == ViolationStatusEnums.PENDING.ToString())
                {
                    if (discipline == null)
                    {
                        // Nếu chưa có thì tạo mới một Discipline tương ứng với Violation đó
                        var disciplineEntity = new Discipline
                        {
                            ViolationId = violation.ViolationId,
                            PennaltyId = null,
                            Description = violation.Name,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now.AddDays(7), 
                            Status = DisciplineStatusEnums.PENDING.ToString()
                        };
                        await _unitOfWork.Discipline.CreateDiscipline(disciplineEntity);
                    }
                }
                else if (violation.Status == ViolationStatusEnums.REJECTED.ToString())
                {
                    if (discipline != null)
                    {
                        // Nếu Discipline tương ứng với Violation đó đã được tạo nhưng Violation bị Rejected dẫn đến Status Discipline = INACTIVE
                        // => Update lại Status của Discipline đó thành PENDING
                        _unitOfWork.Discipline.DetachLocal(discipline, discipline.DisciplineId);
                        discipline.Status = DisciplineStatusEnums.PENDING.ToString();
                        await _unitOfWork.Discipline.UpdateDiscipline(discipline);
                    }
                }

                // Detach the existing tracked instance of the Violation entity
                _unitOfWork.Violation.DetachLocal(violation, violation.ViolationId);

                // Đồng thời cập nhật lại Status của Violation thành APPROVED
                violation.Status = ViolationStatusEnums.APPROVED.ToString();
                _unitOfWork.Violation.Update(violation);

                // Increase NumberOfViolation in StudentInClass
                var studentInClass = await _unitOfWork.StudentInClass.GetStudentInClassById(violation.StudentInClassId.Value);
                if (studentInClass != null)
                {
                    _unitOfWork.StudentInClass.DetachLocal(studentInClass, studentInClass.StudentInClassId);
                    studentInClass.NumberOfViolation = (studentInClass.NumberOfViolation ?? 0) + 1;
                    _unitOfWork.StudentInClass.Update(studentInClass);
                }

                _unitOfWork.Save();

                response.Data = _mapper.Map<ResponseOfViolation>(violation);
                response.Success = true;
                response.Message = "Đã phê duyệt vi phạm thành công.";
            }
            catch (Exception ex)
            {
                response.Message = "Phê duyệt vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<ResponseOfViolation>> RejectViolation(int violationId)
        {
            var response = new DataResponse<ResponseOfViolation>();

            try
            {
                var violation = await _unitOfWork.Violation.GetViolationById(violationId);
                if (violation is null)
                {
                    response.Message = "Không thể tìm thấy Vi phạm!!";
                    response.Success = false;
                    return response;
                }

                // Check if the violation is currently in Approved status
                if (violation.Status == ViolationStatusEnums.APPROVED.ToString())
                {
                    // Detach the existing violation entity to avoid tracking issues
                    _unitOfWork.Violation.DetachLocal(violation, violation.ViolationId);

                    // Update violation status to Rejected
                    violation.Status = ViolationStatusEnums.REJECTED.ToString();
                    await _unitOfWork.Violation.UpdateViolation(violation);

                    // Update Discipline status to INACTIVE
                    var discipline = await _unitOfWork.Discipline.GetDisciplineByViolationId(violation.ViolationId);
                    if (discipline != null)
                    {
                        _unitOfWork.Discipline.DetachLocal(discipline, discipline.DisciplineId);
                        discipline.Status = DisciplineStatusEnums.INACTIVE.ToString();
                        await _unitOfWork.Discipline.UpdateDiscipline(discipline);
                    }

                    // Decrease NumberOfViolation in StudentInClass
                    var studentInClass = _unitOfWork.StudentInClass.GetById(violation.StudentInClassId.Value);
                    if (studentInClass != null && studentInClass.NumberOfViolation > 0)
                    {
                        _unitOfWork.StudentInClass.DetachLocal(studentInClass, studentInClass.StudentInClassId);
                        studentInClass.NumberOfViolation -= 1;
                        _unitOfWork.StudentInClass.Update(studentInClass);
                    }

                    _unitOfWork.Save();

                    response.Data = _mapper.Map<ResponseOfViolation>(violation);
                    response.Success = true;
                    response.Message = "Từ chối vi phạm thành công.";
                }
                else
                {
                    response.Message = "Trạng thái vi phạm đang không phải Approved, Không thể rejected.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Từ chối vi phạm thất bại.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }
            return response;
        }

        public async Task<DataResponse<List<ResponseOfViolation>>> GetViolationsByStudentCode(string studentCode)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();
            try
            {
                var student = await _unitOfWork.Student.GetStudentByCode(studentCode);
                if (student == null)
                {
                    response.Message = "Học sinh không tìm thấy!!";
                    response.Success = false;
                    return response;
                }

                var violations = await _unitOfWork.Violation.GetViolationsByStudentId(student.StudentId);
                response.Data = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Message = "Danh sách vi phạm";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetViolationsByStudentCodeAndYear(string studentCode, short year)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();
            try
            {
                var studentEntity = await _unitOfWork.Student.GetStudentByCode(studentCode);
                if (studentEntity == null)
                {
                    response.Message = "Học sinh không tìm thấy!!";
                    response.Success = false;
                    return response;
                }

                // Find the SchoolYearId based on the studentCode and Year
                var schoolYear = await _unitOfWork.SchoolYear.GetYearBySchoolYearId(studentEntity.SchoolId, year);
                if (schoolYear == null)
                {
                    response.Message = $"Năm học {year} của học sinh không tìm thấy!!";
                    response.Success = false;
                    return response;
                }

                var violations = await _unitOfWork.Violation.GetViolationsByStudentIdAndYear(studentEntity.StudentId, schoolYear.SchoolYearId);
                response.Data = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Message = "Danh sách vi phạm";
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

        public async Task<DataResponse<Dictionary<int, int>>> GetViolationCountByYear(string studentCode)
        {
            var response = new DataResponse<Dictionary<int, int>>();
            try
            {
                var studentEntity = await _unitOfWork.Student.GetStudentByCode(studentCode);
                if (studentEntity == null)
                {
                    response.Message = "Học sinh không tìm thấy!!";
                    response.Success = false;
                    return response;
                }

                var violations = await _unitOfWork.Violation.GetViolationCountByYear(studentEntity.StudentId);
                response.Data = violations;
                response.Message = "Số lượng vi phạm theo năm";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetApprovedViolations()
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetApprovedViolations();
                if (violations is null || !violations.Any())
                {
                    response.Message = "Không tìm thấy vi phạm đã được phê duyệt!!";
                    response.Success = true;
                    return response;
                }

                var violationDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = violationDTO;
                response.Message = "Danh sách các vi phạm được phê duyệt";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetPendingViolations()
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetPendingViolations();
                if (violations is null || !violations.Any())
                {
                    response.Message = "Không tìm thấy vi phạm đang chờ xử lý!!";
                    response.Success = true;
                    return response;
                }

                var violationDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = violationDTO;
                response.Message = "Danh sách vi phạm đang chờ xử lý";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetRejectedViolations()
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetRejectedViolations();
                if (violations is null || !violations.Any())
                {
                    response.Message = "Không tìm thấy vi phạm bị từ chối nào!!";
                    response.Success = true;
                    return response;
                }

                var violationDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = violationDTO;
                response.Message = "Danh sách các vi phạm bị từ chối";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetInactiveViolations()
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetInactiveViolations();
                if (violations is null || !violations.Any())
                {
                    response.Message = "Không tìm thấy vi phạm đã bị xóa nào!!";
                    response.Success = true;
                    return response;
                }

                var violationDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = violationDTO;
                response.Message = "Danh sách vi phạm đã bị xóa";
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

        public async Task<DataResponse<List<ResponseOfViolation>>> GetViolationsBySchoolId(int schoolId)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();
            try
            {
                var violations = await _unitOfWork.Violation.GetViolationsBySchoolId(schoolId);
                if (violations == null || !violations.Any())
                {
                    response.Message = "Không tìm thấy vi phạm nào đối với SchoolId được chỉ định";
                    response.Success = false;
                }
                else
                {
                    var violationDTOS = _mapper.Map<List<ResponseOfViolation>>(violations);
                    response.Data = violationDTOS;
                    response.Message = "Đã tìm thấy vi phạm";
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
        public async Task<DataResponse<List<ResponseOfViolation>>> GetViolationsByMonthAndWeek(int schoolId, short year, int month, int? weekNumber = null)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetViolationsByMonthAndWeek(schoolId, year, month, weekNumber);
                if (violations == null || !violations.Any())
                {
                    response.Data = "Empty";
                    response.Message = weekNumber.HasValue ? "Không có vi phạm nào trong tuần này" : "Không có vi phạm nào trong tháng này";
                    response.Success = true;
                    return response;
                }

                var vioDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = vioDTO;
                response.Message = weekNumber.HasValue ? "Danh sách vi phạm trong tuần" : "Danh sách vi phạm trong tháng";
                response.Success = true;
            }
            catch (ArgumentException ex)
            {
                response.Data = "Empty";
                response.Message = ex.Message;
                response.Success = false;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ResponseOfViolation>>> GetViolationsByYearAndClassName(short year, string className, int schoolId)
        {
            var response = new DataResponse<List<ResponseOfViolation>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetViolationsByYearAndClassName(year, className, schoolId);
                if (violations == null || !violations.Any())
                {
                    response.Data = "Empty";
                    response.Message = "Không có vi phạm nào trong lớp này";
                    response.Success = true;
                    return response;
                }

                var vioDTO = _mapper.Map<List<ResponseOfViolation>>(violations);
                response.Data = vioDTO;
                response.Message = "Danh sách vi phạm trong lớp";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
        public async Task<DataResponse<List<ViolationTypeSummary>>> GetTopFrequentViolations(short year, int schoolId)
        {
            var response = new DataResponse<List<ViolationTypeSummary>>();

            try
            {
                var violations = await _unitOfWork.Violation.GetTopFrequentViolations(year, schoolId);
                if (violations == null || !violations.Any())
                {
                    response.Data = new List<ViolationTypeSummary>();
                    response.Message = "Không có vi phạm thường xuyên trong năm học này";
                    response.Success = true;
                    return response;
                }

                response.Data = violations;
                response.Message = "Danh sách top 3 vi phạm thường xuyên trong năm học";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = new List<ViolationTypeSummary>();
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }

        public async Task<DataResponse<List<ClassViolationSummary>>> GetClassesWithMostViolations(short year, int schoolId)
        {
            var response = new DataResponse<List<ClassViolationSummary>>();

            try
            {
                var classViolations = await _unitOfWork.Violation.GetClassesWithMostViolations(year, schoolId);
                if (classViolations == null || !classViolations.Any())
                {
                    response.Data = "Empty";
                    response.Message = "Không có lớp nào với nhiều vi phạm trong năm học này";
                    response.Success = true;
                    return response;
                }

                response.Data = classViolations;
                response.Message = "Danh sách lớp có nhiều vi phạm trong năm học";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Data = "Empty";
                response.Message = "Oops! Đã có lỗi xảy ra.\n" + ex.Message
                    + (ex.InnerException != null ? ex.InnerException.Message : "");
                response.Success = false;
            }

            return response;
        }
    }
}
