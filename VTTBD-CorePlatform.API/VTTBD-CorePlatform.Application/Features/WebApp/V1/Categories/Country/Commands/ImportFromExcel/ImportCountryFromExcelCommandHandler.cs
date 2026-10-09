using ClosedXML.Excel;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VTTBD_CorePlatform.Application.Abstractions.Localization;
using VTTBD_CorePlatform.Application.Abstractions.MultiTenants;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Specifications.Commands;
using VTTBD_CorePlatform.Domain.Common.Constants.System;

// Đặt biệt danh
using CountryEntity = VTTBD_CorePlatform.Domain.Entities.System.Categories.Country;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.ImportFromExcel;

public sealed class ImportCountryFromExcelCommandHandler : IRequestHandler<ImportCountryFromExcelCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISystemMessageProvider _systemMessageProvider;

    public ImportCountryFromExcelCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentTenant currentTenant,
        ISystemMessageProvider systemMessageProvider)
    {
        _unitOfWork = unitOfWork;
        _currentTenant = currentTenant;
        _systemMessageProvider = systemMessageProvider;
    }

    public async Task<Result> Handle(
        ImportCountryFromExcelCommand request,
        CancellationToken cancellationToken)
    {
        if (request.RequestData.FileBytes == null || request.RequestData.FileBytes.Length == 0)
            return Result.Failure(ErrorCodeConstants.FileEmpty, await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));

        using var stream = new MemoryStream(request.RequestData.FileBytes);
        using var workbook = new XLWorkbook(stream);
        var ws = workbook.Worksheet(1);
        var rows = ws.RowsUsed().Skip(1);

        if (!rows.Any())
        {
            return Result.Failure(
                ErrorCodeConstants.FileEmpty,
                await _systemMessageProvider.GetAsync(ErrorCodeConstants.FileEmpty));
        }

        // Dùng CountryEntity
        var repo = _unitOfWork.Repository<CountryEntity>();

        var spec = new CountryForImportFromExcelSpec();
        var existingEntities = await repo.ListAsync(spec, cancellationToken);

        foreach (var row in rows)
        {
            var name = row.Cell(1).GetString().Trim();

            if (string.IsNullOrEmpty(name))
                return Result.Failure(ErrorCodeConstants.CountryNameRequired, await _systemMessageProvider.GetAsync(ErrorCodeConstants.CountryNameRequired));

            var existing = existingEntities.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (existing == null)
            {
                var entity = new CountryEntity(name);

                await repo.AddAsync(entity, cancellationToken);
                existingEntities.Add(entity);
            }
            else
            {
                existing.Update(name);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}