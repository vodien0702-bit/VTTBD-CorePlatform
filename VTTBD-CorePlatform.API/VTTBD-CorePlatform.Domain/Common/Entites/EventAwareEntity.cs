using System;
using System.Collections.Generic;
using VTTBD_CorePlatform.Domain.Common.Events;

namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class EventAwareEntity<TId> : BaseEntity<TId> where TId : notnull
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected EventAwareEntity() : base() { }
    protected EventAwareEntity(TId id) : base(id) { }

    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null) throw new ArgumentNullException(nameof(domainEvent));
        _domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(DomainEvent domainEvent)
    {
        if (domainEvent is null) return;
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}