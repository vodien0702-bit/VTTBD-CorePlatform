using System;

namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class AuditableEntity<T> : EventAwareEntity<T> where T : notnull
{
    public DateTime CreatedAt { get; protected set; }
    public Guid? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    public string? CreatedIp { get; protected set; }
    public string? UpdatedIp { get; protected set; }
    public string? CreatedUserAgent { get; protected set; }
    public string? UpdatedUserAgent { get; protected set; }

    protected AuditableEntity() : base()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected AuditableEntity(T id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void SetCreated(Guid createdBy, string? ipAddress = null, string? userAgent = null)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        CreatedIp = ipAddress;
        CreatedUserAgent = userAgent;
    }

    public void SetModified(Guid modifiedBy, string? ipAddress = null, string? userAgent = null)
    {
        UpdatedBy = modifiedBy;
        UpdatedAt = DateTime.UtcNow;
        UpdatedIp = ipAddress;
        UpdatedUserAgent = userAgent;
    }
}