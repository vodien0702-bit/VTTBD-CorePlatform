using MediatR;
using System;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.DTOs;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetScheduleBySemester;

public record GetScheduleBySemesterQuery(Guid SemesterId, Guid StudentId)
    : IRequest<Result<RegisteredScheduleSummaryDto>>;