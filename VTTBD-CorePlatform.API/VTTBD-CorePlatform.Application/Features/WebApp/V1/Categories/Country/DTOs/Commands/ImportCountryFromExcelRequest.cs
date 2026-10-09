namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.Categories.Country.DTOs.Commands;

public record ImportCountryFromExcelRequest(byte[] FileBytes, string FileName);