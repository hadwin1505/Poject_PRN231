using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums.Status
{
    public enum ClassGroupStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum SchoolConfigStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum RegisteredSchoolStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum DisciplineStatusEnums
    {
        PENDING,
        EXECUTING,
        DONE,
        INACTIVE,
    }

    public enum EvaluationDetailStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum StudentInClassStatusEnums
    {
        ENROLLED,
        UNENROLLED,
    }

    public enum ViolationStatusEnums
    {
        PENDING,
        APPROVED,
        REJECTED,
        INACTIVE,
    }

    public enum ViolationTypeStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum ViolationGroupStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum ViolationConfigStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum PatrolScheduleStatusEnums
    {
        ONGOING,
        FINISHED,
        INACTIVE,
    }

    public enum PenaltyStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum PackageStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum PackageTypeStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum YearPackageStatusEnums
    {
        VALID,
        EXPIRED,
    }

    public enum SchoolYearStatusEnums
    {
        ONGOING,
        FINISHED,
        INACTIVE
    }

    public enum HighSchoolStatusEnums
    {
        ACTIVE,
        INACTIVE
    }

    public enum AdminStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum UserStatusEnums
    {
        ACTIVE,
        INACTIVE,
    }

    public enum OrderStatusEnum
    {
        PENDING,
        PAID,
        CANCELLED
    }
}
