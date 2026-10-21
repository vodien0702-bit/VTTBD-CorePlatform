using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetAllCourses;

public record CourseItemDto(
    Guid Id,
    Guid StudentId,
    Guid SemesterId,
    string CourseCode,
    string CourseName,
    int Credits,
    string ClassName,
    string Shift,
    string Schedule,
    string LecturerName,
    string RoomName,
    decimal AmountPaid
);

public record GetAllCoursesQuery : IRequest<Result<List<CourseItemDto>>>;

public sealed class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, Result<List<CourseItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllCoursesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CourseItemDto>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        var courseRepo = _unitOfWork.Repository<RegisteredCourse>();
        var courses = await courseRepo.ListAsync(new { }, cancellationToken);

        var dtos = courses.Select(c => new CourseItemDto(
            c.Id,
            c.StudentId,
            c.SemesterId,
            c.CourseCode,
            c.CourseName,
            c.Credits,
            c.ClassName,
            c.Shift,
            c.Schedule,
            c.LecturerName,
            c.RoomName,
            c.AmountPaid
        )).ToList();

        return Result<List<CourseItemDto>>.Success(dtos);
    }
}