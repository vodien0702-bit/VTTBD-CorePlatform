using MediatR;
using System;
using System.Collections.Generic;
using VTTBD_CorePlatform.Application.Common.Models;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetSemesters;

public record SemesterDto(Guid Id, string Name);

public record GetSemestersQuery : IRequest<Result<List<SemesterDto>>>;