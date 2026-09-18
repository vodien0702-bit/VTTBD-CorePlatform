using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Events;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;

public abstract class SoftDeleteEntity<TId> : AuditableEntity<TId>
    where TId : notnull
{
    // ========== SOFT DELETE PROPERTIES ==========

    /// <summary>
    /// Flag đánh dấu entity đã bị soft delete
    /// Mặc định: false (chưa xóa)
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Thời điểm soft delete
    /// Null nếu entity chưa bị xóa
    /// </summary>
    public DateTime? DeletedAt { get; protected set; }

    /// <summary>
    /// Người thực hiện soft delete
    /// Null nếu entity chưa bị xóa
    /// </summary>
    public Guid? DeletedBy { get; protected set; }

    /// <summary>
    /// Lý do soft delete (optional)
    /// </summary>
    public string? DeletionReason { get; protected set; }

    // ========== CONSTRUCTORS ==========

    /// <summary>
    /// Default constructor
    /// </summary>
    protected SoftDeleteEntity() : base()
    {
        IsDeleted = false;
    }

    /// <summary>
    /// Constructor với ID có sẵn
    /// </summary>
    protected SoftDeleteEntity(TId id) : base(id)
    {
        IsDeleted = false;
    }

    // ========== SOFT DELETE METHODS ==========

    /// <summary>
    /// Soft delete entity
    /// </summary>
    /// <param name="deletedBy">Người thực hiện xóa</param>
    /// <param name="reason">Lý do xóa (optional)</param>
    /// <exception cref="ArgumentException">Khi deletedBy null hoặc empty</exception>
    /// <exception cref="InvalidOperationException">Khi entity đã bị xóa</exception>
    public virtual void SoftDelete(Guid deletedBy, string? reason = null)
    {
        if (IsDeleted)
            throw new InvalidOperationException($"Entity {Id} is already deleted");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        DeletionReason = reason;

        SetModified(deletedBy);

        OnSoftDeleted();

        RaiseDomainEvent(new EntitySoftDeletedEvent<TId>(Id));
    }

    /// <summary>
    /// Restore soft deleted entity
    /// </summary>
    /// <exception cref="InvalidOperationException">Khi entity chưa bị xóa</exception>
    protected void Restore(Guid restoredBy)
    {
        if (!IsDeleted)
            throw new InvalidOperationException($"Entity {Id} is not deleted");

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        DeletionReason = null;

        SetModified(restoredBy);

        OnRestored();

        RaiseDomainEvent(new EntityRestoredEvent<TId>(Id));
    }

    /// <summary>
    /// Permanent delete (hard delete)
    /// Sử dụng cẩn thận - không thể khôi phục
    /// </summary>
    public virtual void PermanentDelete(Guid deletedBy, string? reason = null)
    {
        SoftDelete(deletedBy, reason);

        // Thêm logic đặc biệt cho permanent delete nếu cần
        // Ví dụ: gọi domain event, ghi log đặc biệt, etc.
        OnPermanentlyDeleted();
    }

    // ========== PROTECTED HOOK METHODS ==========

    /// <summary>
    /// Hook method được gọi sau khi soft delete
    /// Có thể override để thêm custom logic
    /// </summary>
    protected virtual void OnSoftDeleted()
    {
        // Có thể override để thêm domain events, validation, etc.
    }

    /// <summary>
    /// Hook method được gọi sau khi restore
    /// Có thể override để thêm custom logic
    /// </summary>
    protected virtual void OnRestored()
    {
        // Có thể override để thêm domain events, validation, etc.
    }

    /// <summary>
    /// Hook method được gọi sau khi permanent delete
    /// Có thể override để thêm custom logic
    /// </summary>
    protected virtual void OnPermanentlyDeleted()
    {
        // Có thể override để thêm domain events, validation, etc.
    }

    // ========== BUSINESS METHODS ==========

    /// <summary>
    /// Kiểm tra entity có thể restore không
    /// Mặc định: entity có thể restore trong vòng 30 ngày
    /// Có thể override để thay đổi logic
    /// </summary>
    public virtual bool CanBeRestored()
    {
        if (!IsDeleted || !DeletedAt.HasValue)
            return false;

        var retentionDays = GetRetentionPeriodDays();
        var daysSinceDeletion = (DateTime.UtcNow - DeletedAt.Value).TotalDays;

        return daysSinceDeletion <= retentionDays;
    }

    /// <summary>
    /// Kiểm tra entity đã bị xóa vĩnh viễn chưa
    /// Mặc định: sau 365 ngày coi như xóa vĩnh viễn
    /// Có thể override để thay đổi logic
    /// </summary>
    public virtual bool IsPermanentlyDeleted()
    {
        if (!IsDeleted || !DeletedAt.HasValue)
            return false;

        var permanentDeleteDays = GetPermanentDeletePeriodDays();
        var daysSinceDeletion = (DateTime.UtcNow - DeletedAt.Value).TotalDays;

        return daysSinceDeletion > permanentDeleteDays;
    }

    /// <summary>
    /// Lấy số ngày retention period
    /// Mặc định: 30 ngày
    /// Có thể override để config khác nhau cho từng entity type
    /// </summary>
    protected virtual int GetRetentionPeriodDays()
    {
        return 30; // 30 days retention
    }

    /// <summary>
    /// Lấy số ngày sau đó coi như xóa vĩnh viễn
    /// Mặc định: 365 ngày
    /// Có thể override để config khác nhau cho từng entity type
    /// </summary>
    protected virtual int GetPermanentDeletePeriodDays()
    {
        return 365; // After 1 year, consider permanently deleted
    }

    /// <summary>
    /// Kiểm tra deletion có hợp lệ không
    /// Có thể override để thêm validation logic
    /// </summary>
    protected virtual bool IsDeletionValid(string deletedBy, string? reason)
    {
        return !string.IsNullOrWhiteSpace(deletedBy);
    }

    /// <summary>
    /// Kiểm tra restore có hợp lệ không
    /// Có thể override để thêm validation logic
    /// </summary>
    protected virtual bool IsRestoreValid()
    {
        return IsDeleted && CanBeRestored();
    }
    // ========== VALIDATION ==========

    /// <summary>
    /// Kiểm tra entity có valid không
    /// </summary>
    public override bool IsValid()
    {
        var baseValid = base.IsValid();

        if (IsDeleted)
        {
            return baseValid &&
                   DeletedBy != null &&
                   DeletedAt.HasValue;
        }

        return baseValid;
    }

    /// <summary>
    /// Validate entity và trả về danh sách lỗi
    /// </summary>
    public override IEnumerable<string> Validate()
    {
        var errors = base.Validate().ToList();

        if (IsDeleted)
        {
            if (DeletedBy == null)
                errors.Add("Deleted by is required for deleted entities");

            if (!DeletedAt.HasValue)
                errors.Add("Deletion timestamp is required for deleted entities");
        }

        return errors;
    }

    // ========== EQUALITY OVERRIDES ==========

    /// <summary>
    /// Override equality để include soft delete state
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (!base.Equals(obj))
            return false;

        if (obj is SoftDeleteEntity<TId> other)
        {
            return IsDeleted == other.IsDeleted &&
                   DeletedAt == other.DeletedAt;
        }

        return false;
    }

    /// <summary>
    /// Override hash code để include soft delete state
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), IsDeleted, DeletedAt);
    }

    // ========== TO STRING ==========

    public override string ToString()
    {
        return $"{base.ToString()} [Deleted: {IsDeleted}, DeletedAt: {DeletedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"}]";
    }
}
