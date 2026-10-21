using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetStudents;

/// <summary>
/// DTO chứa thông tin sinh viên (Cho phép Email có thể null)
/// </summary>
public record StudentDto(Guid Id, string StudentCode, string FullName, string? Email);

/// <summary>
/// Query lấy danh sách tất cả sinh viên
/// </summary>
public record GetStudentsQuery : IRequest<Result<List<StudentDto>>>;

/// <summary>
/// Handler xử lý truy vấn danh sách sinh viên
/// </summary>
public sealed class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, Result<List<StudentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<StudentDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
    {
        var studentRepo = _unitOfWork.Repository<Student>();
        var students = await studentRepo.ListAsync(new { }, cancellationToken);

        var dtos = students
            .Select(s => new StudentDto(
                s.Id,
                s.StudentCode,
                s.FullName,
                s.Email
            ))
            .ToList();

        return Result<List<StudentDto>>.Success(dtos);
    }
}