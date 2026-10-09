using System;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

public record UpdateCountryRequest(Guid Id, string Name);