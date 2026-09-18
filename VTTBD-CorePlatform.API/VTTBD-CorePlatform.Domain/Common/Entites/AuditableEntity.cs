using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

public abstract class AuditableEntity<T> : EventAwareEntity<T>
        where T : notnull
{
    // ========== AUDIT PROPERTIES ==========
    public DateTime CreatedAt { get; protected set; }
    public Guid? CreatedBy { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public Guid? UpdatedBy { get; protected set; }

    // Extended audit (optional)
    public string? CreatedIp { get; protected set; }
    public string? UpdatedIp { get; protected set; }
    public string? CreatedUserAgent { get; protected set; }
    public string? UpdatedUserAgent { get; protected set; }

    // ========== CONSTRUCTORS ==========
    protected AuditableEntity() : base()
    {
        CreatedAt = DateTime.UtcNow;
    }

    protected AuditableEntity(T id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }

    // ========== AUDIT METHODS ==========
    protected void SetCreated(Guid createdBy, string? ipAddress = null, string? userAgent = null)
    {
        CreatedBy = createdBy;
        CreatedAt = DateTime.UtcNow;
        CreatedIp = ipAddress;
        CreatedUserAgent = userAgent;
    }

    protected void SetModified(Guid modifiedBy, string? ipAddress = null, string? userAgent = null)
    {
        UpdatedBy = modifiedBy;
        UpdatedAt = DateTime.UtcNow;
        UpdatedIp = ipAddress;
        UpdatedUserAgent = userAgent;
    }
    public override bool IsValid()
    {
        return base.IsValid();
    }
}
