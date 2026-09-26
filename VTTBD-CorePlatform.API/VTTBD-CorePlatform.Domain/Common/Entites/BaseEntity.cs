using System;
using System.Collections.Generic;

namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class BaseEntity<T> where T : notnull
{
    public T Id { get; protected set; } = default!;

    protected BaseEntity() { }

    protected BaseEntity(T id)
    {
        if (id == null || id.Equals(default(T)))
            throw new ArgumentException("ID cannot be null or default", nameof(id));

        Id = id;
    }

    protected virtual void SetId(T id)
    {
        if (id == null || id.Equals(default(T)))
            throw new ArgumentException("ID cannot be null or default", nameof(id));

        Id = id;
    }

    protected virtual void SetDefaultId() { }

    public virtual bool IsNew() => Id == null || Id.Equals(default(T));

    public bool Equals(BaseEntity<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (IsNew() || other.IsNew()) return ReferenceEquals(this, other);
        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj) => Equals(obj as BaseEntity<T>);

    public override int GetHashCode() => IsNew() ? base.GetHashCode() : HashCode.Combine(GetType(), Id);

    public static bool operator ==(BaseEntity<T>? left, BaseEntity<T>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<T>? left, BaseEntity<T>? right) => !(left == right);

    public virtual bool IsValid() => !IsNew();

    public virtual IEnumerable<string> Validate()
    {
        var errors = new List<string>();
        if (IsNew()) errors.Add("Entity ID is required");
        return errors;
    }
}