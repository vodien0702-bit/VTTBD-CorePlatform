using System;
using System.Linq.Expressions;
using RegisteredCourseEntity = VTTBD_CorePlatform.Domain.Entities.StudentManagement.RegisteredCourse;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Specifications.Queries;

public class ScheduleBySemesterSpec
{
    public Expression<Func<RegisteredCourseEntity, bool>> Criteria { get; }

    public ScheduleBySemesterSpec(Guid studentId, Guid semesterId)
    {
        Criteria = x => x.StudentId == studentId
                     && x.SemesterId == semesterId
                     && !x.IsDeleted;
    }
}