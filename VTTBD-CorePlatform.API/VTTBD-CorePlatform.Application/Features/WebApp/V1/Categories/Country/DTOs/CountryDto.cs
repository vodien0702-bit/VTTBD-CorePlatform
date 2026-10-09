using System;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs;

public class CountryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}