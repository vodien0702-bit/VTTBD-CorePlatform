using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;
using VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.Commands.Create;

public sealed record CreateCountryCommand(CreateCountryRequest RequestData) : IRequest<Result<CountryDto>>;