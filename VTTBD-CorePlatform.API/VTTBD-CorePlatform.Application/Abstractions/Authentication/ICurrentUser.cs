using System;

namespace VTTBD_CorePlatform.Application.Abstractions.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }
    string? UserName { get; }
}