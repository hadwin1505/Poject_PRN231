using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        IAdminRepository Admin { get; }
        IClassGroupRepository ClassGroup {  get; }
        IClassReposirory Class {  get; }
        IDisciplineRepository Discipline {  get; }
        IEvaluationDetailRepository EvaluationDetail { get; }
        IEvaluationRepository Evaluation {  get; }
        IImageUrlRepository ImageUrl { get; }
        IPackageRepository Package { get; }
        IPackageTypeRepository PackageType { get; }
        IPatrolScheduleRepository PatrolSchedule { get; }
        IPenaltyRepository Penalty { get; }
        IRegisteredSchoolRepository RegisteredSchool { get; }
        IHighSchoolRepository HighSchool { get; }
        ISchoolYearRepository SchoolYear { get; }
        IOrderRepository Order { get; }
        IStudentInClassRepository StudentInClass { get; }
        IStudentRepository Student {  get; }
        IStudentSupervisorRepository StudentSupervisor { get; }
        ITeacherRepository Teacher { get; }
        IUserRepository User { get; }
        IViolationConfigRepository ViolationConfig { get; }
        IViolationGroupRepository ViolationGroup { get; }
        IViolationRepository Violation { get; }
        IViolationTypeRepository ViolationType { get; }
        IYearPackageRepository YearPackage { get; }

        IDbContextTransaction StartTransaction(string name);
        void StopTransaction(IDbContextTransaction commit);
        void RollBack(IDbContextTransaction commit, string name);
        int Save();
    }
}
