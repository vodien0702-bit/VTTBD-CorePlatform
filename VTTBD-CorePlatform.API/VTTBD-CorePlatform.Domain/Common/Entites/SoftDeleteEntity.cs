using System;
using System.Collections.Generic;
using System.Linq;
using VTTBD_CorePlatform.Domain.Common.Events;

namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class SoftDeleteEntity<TId> : AuditableEntity<TId> where TId : notnull
{
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    public string? DeletionReason { get; protected set; }

    protected SoftDeleteEntity() : base()
    {
        IsDeleted = false;
    }

    protected SoftDeleteEntity(TId id) : base(id)
    {
        IsDeleted = false;
    }

    public virtual void SoftDelete(Guid deletedBy, string? reason = null)
    {
        if (IsDeleted) throw new InvalidOperationException($"Entity {Id} is already deleted");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        DeletionReason = reason;

        SetModified(deletedBy);
        OnSoftDeleted();
        RaiseDomainEvent(new EntitySoftDeletedEvent<TId>(Id));
    }

    public virtual void Restore(Guid restoredBy)
    {
        if (!IsDeleted) throw new InvalidOperationException($"Entity {Id} is not deleted");

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        DeletionReason = null;

        SetModified(restoredBy);
        OnRestored();
        RaiseDomainEvent(new EntityRestoredEvent<TId>(Id));
    }

    protected virtual void OnSoftDeleted() { }
    protected virtual void OnRestored() { }
}