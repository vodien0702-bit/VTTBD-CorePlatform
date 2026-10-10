using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.Queries.GetSemesters;

public sealed class GetSemestersQueryHandler : IRequestHandler<GetSemestersQuery, Result<List<SemesterDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSemestersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<SemesterDto>>> Handle(GetSemestersQuery request, CancellationToken cancellationToken)
    {
        var semesterRepo = _unitOfWork.Repository<Semester>();
        // Lấy tất cả học kỳ chưa bị xóa
        var semesters = await semesterRepo.ListAsync(new { }, cancellationToken);

        var dtos = semesters.Select(s => new SemesterDto(s.Id, s.Name)).ToList();
        return Result<List<SemesterDto>>.Success(dtos);
    }
}