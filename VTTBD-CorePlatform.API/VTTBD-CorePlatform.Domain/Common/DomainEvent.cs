using System;

namespace VTTBD_CorePlatform.Domain.Common.Events;

public abstract class DomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
}

public class EntitySoftDeletedEvent<TId> : DomainEvent
{
    public TId Id { get; }
    public EntitySoftDeletedEvent(TId id) => Id = id;
}

public class EntityRestoredEvent<TId> : DomainEvent
{
    public TId Id { get; }
    public EntityRestoredEvent(TId id) => Id = id;
}