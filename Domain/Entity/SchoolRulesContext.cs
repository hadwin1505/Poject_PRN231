using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.Entity;

public partial class SchoolRulesContext : DbContext
{
    public SchoolRulesContext()
    {
    }

    public SchoolRulesContext(DbContextOptions<SchoolRulesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassGroup> ClassGroups { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<EvaluationDetail> EvaluationDetails { get; set; }

    public virtual DbSet<HighSchool> HighSchools { get; set; }

    public virtual DbSet<ImageUrl> ImageUrls { get; set; }
    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<PackageType> PackageTypes { get; set; }

    public virtual DbSet<PatrolSchedule> PatrolSchedules { get; set; }

    public virtual DbSet<Penalty> Penalties { get; set; }

    public virtual DbSet<RegisteredSchool> RegisteredSchools { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SchoolYear> SchoolYears { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentInClass> StudentInClasses { get; set; }

    public virtual DbSet<StudentSupervisor> StudentSupervisors { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    public virtual DbSet<ViolationConfig> ViolationConfigs { get; set; }

    public virtual DbSet<ViolationGroup> ViolationGroups { get; set; }

    public virtual DbSet<ViolationType> ViolationTypes { get; set; }

    public virtual DbSet<YearPackage> YearPackages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(GetConnectionString());

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, false)
            .Build();
        var strConn = config.GetConnectionString("DefaultConnection");
        return strConn;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Admins)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admin_Role");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassGroupId).HasColumnName("ClassGroupID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Grade).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(70);
            entity.Property(e => e.SchoolYearId).HasColumnName("SchoolYearID");

            entity.HasOne(d => d.ClassGroup).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ClassGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_ClassGroup");

            entity.HasOne(d => d.SchoolYear).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SchoolYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_SchoolYear");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Classes)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Class_Teacher");
        });

        modelBuilder.Entity<ClassGroup>(entity =>
        {
            entity.ToTable("ClassGroup");

            entity.Property(e => e.ClassGroupId).HasColumnName("ClassGroupID");
            entity.Property(e => e.Hall).HasMaxLength(20);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.ClassGroups)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_ClassGroup_HighSchool");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.ToTable("Discipline");

            entity.Property(e => e.DisciplineId).HasColumnName("DisciplineID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.PennaltyId).HasColumnName("PennaltyID");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ViolationId).HasColumnName("ViolationID");

            entity.HasOne(d => d.Pennalty).WithMany(p => p.Disciplines)
                .HasForeignKey(d => d.PennaltyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Discipline_Penalty");

            entity.HasOne(d => d.Violation).WithMany(p => p.Disciplines)
                .HasForeignKey(d => d.ViolationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Discipline_Violation");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.ToTable("Evaluation");

            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.From).HasColumnType("date");
            entity.Property(e => e.SchoolYearId).HasColumnName("SchoolYearID");
            entity.Property(e => e.To).HasColumnType("date");
            entity.Property(e => e.ViolationConfigId).HasColumnName("ViolationConfigID");

            entity.HasOne(d => d.SchoolYear).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.SchoolYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evaluation_SchoolYear");

            entity.HasOne(d => d.ViolationConfig).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.ViolationConfigId)
                .HasConstraintName("FK_Evaluation_ViolationConfig");
        });

        modelBuilder.Entity<EvaluationDetail>(entity =>
        {
            entity.HasKey(e => e.EvaluationDetailId).HasName("PK_EvaluationDetail_1");

            entity.ToTable("EvaluationDetail");

            entity.Property(e => e.EvaluationDetailId).HasColumnName("EvaluationDetailID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.EvaluationId).HasColumnName("EvaluationID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Class).WithMany(p => p.EvaluationDetails)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvaluationDetail_Class");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.EvaluationDetails)
                .HasForeignKey(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EvaluationDetail_Evaluation");
        });

        modelBuilder.Entity<HighSchool>(entity =>
        {
            entity.HasKey(e => e.SchoolId);

            entity.ToTable("HighSchool");

            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(12);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.WebUrl)
                .HasMaxLength(500)
                .HasColumnName("WebURL");
        });

        modelBuilder.Entity<ImageUrl>(entity =>
        {
            entity.ToTable("ImageURL");

            entity.Property(e => e.ImageUrlid).HasColumnName("ImageURLID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("URL");
            entity.Property(e => e.ViolationId).HasColumnName("ViolationID");

            entity.HasOne(d => d.Violation).WithMany(p => p.ImageUrls)
                .HasForeignKey(d => d.ViolationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImageURL_Violation");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CounterAccountBankName).HasMaxLength(100);
            entity.Property(e => e.CounterAccountName).HasMaxLength(100);
            entity.Property(e => e.CounterAccountNumber).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Package).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Package");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("Package");

            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.PackageType).WithMany(p => p.Packages)
                .HasForeignKey(d => d.PackageTypeId)
                .HasConstraintName("FK_Package_PackageType");
        });

        modelBuilder.Entity<PackageType>(entity =>
        {
            entity.ToTable("PackageType");

            entity.Property(e => e.PackageTypeId).HasColumnName("PackageTypeID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<PatrolSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId);

            entity.ToTable("PatrolSchedule");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.From).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SupervisorId).HasColumnName("SupervisorID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.To).HasColumnType("date");

            entity.HasOne(d => d.Class).WithMany(p => p.PatrolSchedules)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatrolSchedule_Class");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.PatrolSchedules)
                .HasForeignKey(d => d.SupervisorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatrolSchedule_StudentSupervisor1");

            entity.HasOne(d => d.Teacher).WithMany(p => p.PatrolSchedules)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PatrolSchedule_Teacher");
        });

        modelBuilder.Entity<Penalty>(entity =>
        {
            entity.HasKey(e => e.PenaltyId).HasName("PK_ActivityType");

            entity.ToTable("Penalty");

            entity.Property(e => e.PenaltyId).HasColumnName("PenaltyID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.Penalties)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Penalty_HighSchool");
        });

        modelBuilder.Entity<RegisteredSchool>(entity =>
        {
            entity.HasKey(e => e.RegisteredId);

            entity.ToTable("RegisteredSchool");

            entity.Property(e => e.RegisteredId).HasColumnName("RegisteredID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RegisteredDate).HasColumnType("date");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.RegisteredSchools)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RegisteredSchool_HighSchool");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SchoolYear>(entity =>
        {
            entity.ToTable("SchoolYear");

            entity.Property(e => e.SchoolYearId).HasColumnName("SchoolYearID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.SchoolYears)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SchoolYear_HighSchool");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Student");

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(12);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

            entity.HasOne(d => d.School).WithMany(p => p.Students)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_HighSchool");
        });

        modelBuilder.Entity<StudentInClass>(entity =>
        {
            entity.ToTable("StudentInClass");

            entity.Property(e => e.StudentInClassId).HasColumnName("StudentInClassID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.EnrollDate).HasColumnType("date");
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Class).WithMany(p => p.StudentInClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentInClass_Class");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentInClasses)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentInClass_Student");
        });

        modelBuilder.Entity<StudentSupervisor>(entity =>
        {
            entity.ToTable("StudentSupervisor");

            entity.Property(e => e.StudentSupervisorId).HasColumnName("StudentSupervisorID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.StudentInClassId).HasColumnName("StudentInClassID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.StudentSupervisors)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentSupervisor_User");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.ToTable("Teacher");

            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.School).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.SchoolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_HighSchool");

            entity.HasOne(d => d.User).WithMany(p => p.Teachers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teacher_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Entity");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(70);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");

            entity.HasOne(d => d.School).WithMany(p => p.Users)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_User_HighSchool");
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.ToTable("Violation");

            entity.Property(e => e.ViolationId).HasColumnName("ViolationID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.CreatedAt).HasColumnType("date");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentInClassId).HasColumnName("StudentInClassID");
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");
            entity.Property(e => e.UpdatedAt).HasColumnType("date");
            entity.Property(e => e.ViolationTypeId).HasColumnName("ViolationTypeID");

            entity.HasOne(d => d.Class).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Violation_Class");

            entity.HasOne(d => d.StudentInClass).WithMany(p => p.Violations)
                .HasForeignKey(d => d.StudentInClassId)
                .HasConstraintName("FK_Violation_StudentInClass");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Violations)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("FK_Violation_Teacher");

            entity.HasOne(d => d.ViolationType).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ViolationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Violation_ViolationType");
        });

        modelBuilder.Entity<ViolationConfig>(entity =>
        {
            entity.ToTable("ViolationConfig");

            entity.Property(e => e.ViolationConfigId).HasColumnName("ViolationConfigID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ViolationTypeId).HasColumnName("ViolationTypeID");

            entity.HasOne(d => d.ViolationType).WithMany(p => p.ViolationConfigs)
                .HasForeignKey(d => d.ViolationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViolationConfig_ViolationType");
        });

        modelBuilder.Entity<ViolationGroup>(entity =>
        {
            entity.ToTable("ViolationGroup");

            entity.Property(e => e.ViolationGroupId).HasColumnName("ViolationGroupID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.School).WithMany(p => p.ViolationGroups)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_ViolationGroup_HighSchool");
        });

        modelBuilder.Entity<ViolationType>(entity =>
        {
            entity.ToTable("ViolationType");

            entity.Property(e => e.ViolationTypeId).HasColumnName("ViolationTypeID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ViolationGroupId).HasColumnName("ViolationGroupID");

            entity.HasOne(d => d.ViolationGroup).WithMany(p => p.ViolationTypes)
                .HasForeignKey(d => d.ViolationGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ViolationType_ViolationGroup");
        });

        modelBuilder.Entity<YearPackage>(entity =>
        {
            entity.ToTable("YearPackage");

            entity.Property(e => e.YearPackageId).HasColumnName("YearPackageID");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.SchoolYearId).HasColumnName("SchoolYearID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Package).WithMany(p => p.YearPackages)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_YearPackage_Package");

            entity.HasOne(d => d.SchoolYear).WithMany(p => p.YearPackages)
                .HasForeignKey(d => d.SchoolYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_YearPackage_SchoolYear");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
