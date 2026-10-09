using System.Threading.Tasks;

namespace VTTBD_CorePlatform.Application.Abstractions.Localization;

public interface ISystemMessageProvider
{
    Task<string> GetAsync(string code);
}