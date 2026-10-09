using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.ImportFromExcel;

public sealed record ImportCountryFromExcelCommand(ImportCountryFromExcelRequest RequestData) : IRequest<Result>;