using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Localization;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using VTTBD_CorePlatform.Domain.Common.Constants.System;

// 1. Đặt biệt danh CountryEntity để C# không bị nhầm với namespace Country
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Create;

public sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result<CountryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public CreateCountryCommandHandler(
        IUnitOfWork unitOfWork,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<CountryDto>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        // 2. Dùng CountryEntity ở đây thay vì Country
        var repo = _unitOfWork.Repository<CountryEntity>();

        var spec = new CountryByNameForCreateSpec(request.RequestData.Name);

        var existing = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (existing != null)
            return Result<CountryDto>.Failure(
                ErrorCodeConstants.CountryNameExists,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNameExists));

        // 3. Khởi tạo đối tượng bằng CountryEntity
        var entity = new CountryEntity(request.RequestData.Name);

        await repo.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CountryDto>.Success(new CountryDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}