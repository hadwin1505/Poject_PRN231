using Azure.Core;
using Domain.Entity;
using Infrastructures.Interfaces;
using Infrastructures.Interfaces.IUnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly SchoolRulesContext _context;
        public UnitOfWork(SchoolRulesContext context)
        {
            _context = context;
            Admin = new AdminRepository(_context);
            ClassGroup = new ClassGroupRepository(_context);
            Class = new ClassRepository(_context);
            Discipline = new DisciplineRepository(_context);
            EvaluationDetail = new EvaluationDetailRepository(_context);
            Evaluation = new EvaluationRepository(_context);
            ImageUrl = new ImageUrlRepository(_context);
            Package = new PackageRepository(_context);
            PackageType = new PackageTypeRepository(_context);
            PatrolSchedule = new PatrolScheduleRepository(_context);
            Penalty = new PenaltyRepository(_context);
            RegisteredSchool = new RegisteredSchoolRepository(_context);
            HighSchool = new HighSchoolRepository(_context);
            SchoolYear = new SchoolYearRepository(_context);
            Order = new OrderRepository(_context);
            StudentInClass = new StudentInClassRepository(_context);
            Student = new StudentRepository(_context);
            StudentSupervisor = new StudentSupervisorRepository(_context);
            Teacher = new TeacherRepository(_context);
            User = new UserRepository(_context);
            ViolationConfig = new ViolationConfigRepository(_context);
            ViolationGroup = new ViolationGroupRepository(_context);
            Violation = new ViolationRepository(_context);
            ViolationType = new ViolationTypeRepository(_context);
            YearPackage = new YearPackageRepository(_context);
        }
        public SchoolRulesContext Context { get { return _context; } }
        public IAdminRepository Admin { get; }
        public IClassGroupRepository ClassGroup { get; }
        public IClassReposirory Class { get; }
        public IDisciplineRepository Discipline { get; }
        public IEvaluationDetailRepository EvaluationDetail { get; }
        public IEvaluationRepository Evaluation { get; }
        public IImageUrlRepository ImageUrl { get; }
        public IPackageRepository Package { get; }
        public IPackageTypeRepository PackageType { get; }
        public IPatrolScheduleRepository PatrolSchedule { get; }
        public IPenaltyRepository Penalty { get; }
        public IRegisteredSchoolRepository RegisteredSchool { get; }
        public IHighSchoolRepository HighSchool { get; }
        public ISchoolYearRepository SchoolYear { get; }
        public IOrderRepository Order { get; }
        public IStudentInClassRepository StudentInClass { get; }
        public IStudentRepository Student { get; }
        public IStudentSupervisorRepository StudentSupervisor { get; }
        public ITeacherRepository Teacher { get; }
        public IUserRepository User { get; }
        public IViolationConfigRepository ViolationConfig { get; }
        public IViolationGroupRepository ViolationGroup { get; }
        public IViolationRepository Violation { get; }
        public IViolationTypeRepository ViolationType { get; }
        public IYearPackageRepository YearPackage { get; }

        public void RollBack(IDbContextTransaction commit, string name)
        {
            commit.RollbackToSavepoint(name);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public IDbContextTransaction StartTransaction(string name)
        {
            var commit = _context.Database.BeginTransaction();
            commit.CreateSavepoint(name);
            return commit;
        }

        public void StopTransaction(IDbContextTransaction commit)
        {
            commit.Commit();
        }
    }
}
