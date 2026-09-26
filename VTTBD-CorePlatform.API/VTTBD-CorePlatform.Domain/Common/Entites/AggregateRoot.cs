namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class AggregateRoot<TId> : SoftDeleteEntity<TId> where TId : notnull
{
    protected AggregateRoot() : base() { }
    protected AggregateRoot(TId id) : base(id) { }
}