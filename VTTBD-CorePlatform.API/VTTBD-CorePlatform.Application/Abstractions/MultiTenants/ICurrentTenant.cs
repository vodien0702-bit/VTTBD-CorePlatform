using System;

namespace VTTBD_CorePlatform.Application.Abstractions.MultiTenants;

public interface ICurrentTenant
{
    Guid? TenantId { get; }
}