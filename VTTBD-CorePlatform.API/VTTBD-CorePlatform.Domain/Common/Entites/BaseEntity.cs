using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VTTBD_CorePlatform.Domain.Common.Entities;

public abstract class BaseEntity<T>
        where T : notnull
{
    public T Id { get; protected set; } = default!;

    /// <summary>
    /// Default constructor
    /// </summary>
    protected BaseEntity()
    {
        // Derived classes nên override SetDefaultId() nếu cần auto-generate ID
    }

    /// <summary>
    /// Constructor với ID có sẵn
    /// </summary>
    protected BaseEntity(T id)
    {
        if (id == null || id.Equals(default(T)))
            throw new ArgumentException("ID cannot be null or default", nameof(id));

        Id = id;
    }

    #region ID Management

    /// <summary>
    /// Set ID (cho trường hợp đặc biệt)
    /// </summary>
    protected virtual void SetId(T id)
    {
        if (id == null || id.Equals(default(T)))
            throw new ArgumentException("ID cannot be null or default", nameof(id));

        Id = id;
    }

    /// <summary>
    /// Set default ID (cho auto-generation)
    /// Mặc định không làm gì, derived classes nên override
    /// </summary>
    protected virtual void SetDefaultId()
    {
        // Default: không làm gì
        // Ví dụ: Guid.NewGuid(), sequence.nextval(), etc.
    }

    /// <summary>
    /// Kiểm tra entity có mới không (chưa được persist)
    /// </summary>
    public virtual bool IsNew()
    {
        return Id == null || Id.Equals(default(T));
    }

    #endregion

    #region Equality Implementation

    /// <summary>
    /// Entities bằng nhau khi có cùng type và ID
    /// </summary>
    public bool Equals(BaseEntity<T>? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        if (IsNew() || other.IsNew())
            return ReferenceEquals(this, other);

        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseEntity<T>);
    }

    public override int GetHashCode()
    {
        return IsNew()
            ? base.GetHashCode()
            : HashCode.Combine(GetType(), Id);
    }

    public static bool operator ==(BaseEntity<T>? left, BaseEntity<T>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity<T>? left, BaseEntity<T>? right)
    {
        return !(left == right);
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Kiểm tra entity có valid không
    /// </summary>
    public virtual bool IsValid()
    {
        return !IsNew();
    }

    /// <summary>
    /// Validate entity và trả về danh sách lỗi
    /// </summary>
    public virtual IEnumerable<string> Validate()
    {
        var errors = new List<string>();

        if (IsNew())
            errors.Add("Entity ID is required");

        return errors;
    }

    /// <summary>
    /// Copy ID từ entity khác (cùng type)
    /// </summary>
    protected virtual void CopyIdFrom(BaseEntity<T> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        if (GetType() != source.GetType())
            throw new ArgumentException($"Cannot copy ID from {source.GetType().Name} to {GetType().Name}");

        Id = source.Id;
    }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Factory method tạo entity mới
    /// </summary>
    public static TEntity Create<TEntity>()
        where TEntity : BaseEntity<T>, new()
    {
        var entity = new TEntity();
        entity.SetDefaultId();
        return entity;
    }

    /// <summary>
    /// Factory method tạo entity với ID
    /// </summary>
    public static TEntity CreateWithId<TEntity>(T id)
        where TEntity : BaseEntity<T>, new()
    {
        var entity = new TEntity();
        entity.SetId(id);
        return entity;
    }

    #endregion
}
