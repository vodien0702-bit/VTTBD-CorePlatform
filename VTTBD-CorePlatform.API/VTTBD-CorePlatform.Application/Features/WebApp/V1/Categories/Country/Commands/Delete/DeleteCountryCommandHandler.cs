using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Authentication;
using VTTBD_CorePlatform.Application.Abstractions.Localization;
using VTTBD_CorePlatform.Application.Abstractions.MultiTenants;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using VTTBD_CorePlatform.Domain.Common.Constants.System;

// Đặt biệt danh
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Delete;

public sealed class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public DeleteCountryCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        // Dùng CountryEntity
        var repo = _unitOfWork.Repository<CountryEntity>();

        var spec = new CountryByIdsForDeleteSpec(request.RequestData.Ids);

        var entities = await repo.ListAsync(spec, cancellationToken);

        if (entities == null || entities.Count == 0)
            return Result.Failure(
                ErrorCodeConstants.CountryNotFound,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNotFound));

        if (entities.Count != request.RequestData.Ids.Count)
            return Result.Failure(
                ErrorCodeConstants.OneOrMoreCountryIdsDoNotExist,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.OneOrMoreCountryIdsDoNotExist));

        foreach (var entity in entities)
        {
            entity.SoftDelete(_currentUser.UserId);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}