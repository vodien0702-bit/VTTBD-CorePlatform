using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.DTOs;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Specifications.Queries;

// Đặt alias để tránh xung đột tên
using RegisteredCourseEntity = VTTBD_CorePlatform.Domain.Entities.StudentManagement.RegisteredCourse;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetScheduleBySemester;

public sealed class GetScheduleBySemesterQueryHandler
    : IRequestHandler<GetScheduleBySemesterQuery, Result<RegisteredScheduleSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetScheduleBySemesterQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RegisteredScheduleSummaryDto>> Handle(
        GetScheduleBySemesterQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository<RegisteredCourseEntity>();

        // Truy vấn lấy danh sách môn học đã đăng ký
        var spec = new ScheduleBySemesterSpec(request.StudentId, request.SemesterId);
        var courses = await repo.ListAsync(spec, cancellationToken);

        // Tính toán tổng số học phần, tổng số tín chỉ và tổng tiền
        var result = new RegisteredScheduleSummaryDto
        {
            TotalCourses = courses.Count,
            TotalCredits = courses.Sum(x => x.Credits),
            TotalAmount = courses.Sum(x => x.AmountPaid),
            Courses = courses.Select(x => new RegisteredCourseItemDto
            {
                Id = x.Id,
                CourseType = x.CourseType,
                CourseCode = x.CourseCode,
                CourseName = x.CourseName,
                Credits = x.Credits,
                ClassName = x.ClassName,
                Shift = x.Shift,
                Schedule = x.Schedule,
                LecturerName = x.LecturerName,
                RoomName = x.RoomName,
                UnitPrice = x.UnitPrice,
                AmountPaid = x.AmountPaid,
                IsApproved = x.IsApproved
            }).ToList()
        };

        return Result<RegisteredScheduleSummaryDto>.Success(result);
    }
}