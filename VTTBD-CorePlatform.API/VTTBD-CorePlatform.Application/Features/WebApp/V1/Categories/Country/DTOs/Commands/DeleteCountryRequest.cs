using System;
using System.Collections.Generic;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

public record DeleteCountryRequest(List<Guid> Ids);