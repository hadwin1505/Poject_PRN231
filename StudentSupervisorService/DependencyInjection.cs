using Domain.Entity;
using Infrastructures.Interfaces.IUnitOfWork;
using Infrastructures.Interfaces;
using Infrastructures.Repository.UnitOfWork;
using Infrastructures.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentSupervisorService.Service;
using StudentSupervisorService.Service.Implement;
using StudentSupervisorService.Mapper;
using Microsoft.EntityFrameworkCore.Internal;
using StudentSupervisorService.CloudinaryConfig;
using StudentSupervisorService.Authentication;
using StudentSupervisorService.Authentication.Implement;

namespace StudentSupervisorService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Database
            services.AddDbContext<SchoolRulesContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.Configure<CloudinarySetting>(options =>
            {
                options.CloudName = configuration.GetSection("CloudinarySetting:CloudName").Value;
                options.ApiKey = configuration.GetSection("CloudinarySetting:ApiKey").Value;
                options.ApiSecret = configuration.GetSection("CloudinarySetting:ApiSecret").Value;
            });

            //Add DI Container
            services.AddTransient<PayOSConfig.PayOSConfig>();
            services.AddTransient<CheckoutService, CheckoutImplement>();
            services.AddTransient<OrderService, OrderImplement>();

            services.AddTransient<IAuthentication, AuthenticationImp>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IAdminRepository, AdminRepository>();
            services.AddTransient<AdminService, AdminImplement>();

            services.AddTransient<IClassGroupRepository, ClassGroupRepository>();
            services.AddTransient<ClassGroupService, ClassGroupImplement>();

            services.AddTransient<IClassReposirory, ClassRepository>();
            services.AddTransient<ClassService, ClassImplement>();

            services.AddTransient<IDisciplineRepository, DisciplineRepository>();
            services.AddTransient<DisciplineService, DisciplineImplement>();

            services.AddTransient<IEvaluationRepository, EvaluationRepository>();
            services.AddTransient<EvaluationService, EvaluationImplement>();

            services.AddTransient<IEvaluationDetailRepository, EvaluationDetailRepository>();
            services.AddTransient<EvaluationDetailService, EvaluationDetailImplement>();

            services.AddTransient<IHighSchoolRepository, HighSchoolRepository>();
            services.AddTransient<HighSchoolService, HighSchoolImplement>();

            services.AddTransient<IImageUrlRepository, ImageUrlRepository>();
            services.AddTransient<ImageUrlService, ImageUrlImplement>();

            services.AddTransient<IPackageRepository, PackageRepository>();
            services.AddTransient<PackageService, PackageImplement>();

            services.AddTransient<IPackageTypeRepository, PackageTypeRepository>();
            services.AddTransient<PackageTypeService, PackageTypeImplement>();

            services.AddTransient<IPatrolScheduleRepository, PatrolScheduleRepository>();
            services.AddTransient<PatrolScheduleService, PatrolScheduleImplement>();

            services.AddTransient<IPenaltyRepository, PenaltyRepository>();
            services.AddTransient<PenaltyService, PenaltyImplement>();

            services.AddTransient<IRegisteredSchoolRepository, RegisteredSchoolRepository>();
            services.AddTransient<RegisteredSchoolService, RegisteredSchoolImplement>();

            services.AddTransient<ISchoolYearRepository, SchoolYearRepository>();
            services.AddTransient<SchoolYearService, SchoolYearImplement>();

            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<StudentService, StudentImplement>();

            services.AddTransient<IStudentInClassRepository, StudentInClassRepository>();
            services.AddTransient<StudentInClassService, StudentInClassImplement>();

            services.AddTransient<IStudentSupervisorRepository, StudentSupervisorRepository>();
            services.AddTransient<StudentSupervisorServices, StudentSupervisorImplement>();

            services.AddTransient<ITeacherRepository, TeacherRepository>();
            services.AddTransient<TeacherService, TeacherImplement>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserService, UserImplement>();

            services.AddTransient<IViolationRepository, ViolationRepository>();
            services.AddTransient<ViolationService, ViolationImplement>();

            services.AddTransient<IViolationConfigRepository, ViolationConfigRepository>();
            services.AddTransient<ViolationConfigService, ViolationConfigImplement>();

            services.AddTransient<IViolationGroupRepository, ViolationGroupRepository>();
            services.AddTransient<ViolationGroupService, ViolationGroupImplement>();

            services.AddTransient<IViolationTypeRepository, ViolationTypeRepository>();
            services.AddTransient<ViolationTypeService, ViolationTypeImplement>();

            services.AddTransient<IYearPackageRepository, YearPackageRepository>();
            services.AddTransient<YearPackageService, YearPackageImplement>();


            services.AddTransient<IServiceCollection, ServiceCollection>();

            // Configure other services
            ConfigureTokenBlacklist(services);
            ConfigureLoginService(services);

            //AUTOMAPPER
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            return services;
        }
        public static void ConfigureTokenBlacklist(IServiceCollection services)
        {
            services.AddSingleton<TokenBlacklistService, TokenBlacklistImplement>();
        }

        public static void ConfigureLoginService(IServiceCollection services)
        {
            services.AddScoped<LoginService, LoginImplement>();
        }
    }
}