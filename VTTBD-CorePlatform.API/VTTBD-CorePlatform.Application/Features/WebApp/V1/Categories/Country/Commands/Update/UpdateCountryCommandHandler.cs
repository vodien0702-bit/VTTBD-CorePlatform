using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Localization;
using VTTBD_CorePlatform.Application.Abstractions.MultiTenants;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using VTTBD_CorePlatform.Domain.Common.Constants.System;

// Đặt biệt danh
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Update;

public sealed class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Result<CountryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public UpdateCountryCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result<CountryDto>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        // Dùng CountryEntity
        var repo = _unitOfWork.Repository<CountryEntity>();

        var spec = new CountryByIdForUpdateSpec(request.RequestData.Id);

        var entity = await repo.FirstOrDefaultAsync(spec, cancellationToken);

        if (entity == null || entity.IsDeleted)
            return Result<CountryDto>.Failure(
                ErrorCodeConstants.CountryNotFound,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNotFound));

        entity.Update(request.RequestData.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CountryDto>.Success(new CountryDto
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }
}