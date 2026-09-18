using VTTBD_CorePlatform.CorePlatform.Domain.Common.Constants;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Enums;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Events;
using VTTBD_CorePlatform.CorePlatform.Domain.Entities.Localization;
using VTTBD_CorePlatform.CorePlatform.Domain.Entities.StudentManagement;
using VTTBD_CorePlatform.CorePlatform.Domain.Events.Domain.Tenant;
using VTTBD_CorePlatform.CorePlatform.Domain.Events.Domain.User;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;
using VTTBD_CorePlatform.Domain.Common.Enums;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatformTBDUni.CorePlatform.Domain.Entities.Identity;

public class User : AggregateRoot<Guid>
{
    public string UserName { get; private set; } = default!;
    public string PasswordHash { get; private set; } = default!;
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public bool PhoneNumberConfirmed { get; private set; }
    public string? SecurityStamp { get; private set; }
    public UserStatus Status { get; private set; } = UserStatus.PendingActivation;
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? LockoutEnd { get; private set; }
    public int FailedLoginAttempts { get; private set; }
    public Guid? StudentId { get; private set; }

    public bool IsSystemAdmin { get; private set; } = false;





    public string UserType { get; private set; } = UserTypeConstants.User;
    public Guid? LanguageId { get; private set; }





    // Navigation properties
    public ICollection<UserTenant> UserTenants { get; private set; } = new List<UserTenant>();
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    public Language? Language { get; private set; }
    public Student? Student { get; private set; }

    // Constructor
    private User() { }

    public User(string email, string passwordHash,
                string? firstName, string? lastName,
                bool isSystemAdmin = false)
    {
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;

        FirstName = firstName;
        LastName = lastName;

        IsSystemAdmin = isSystemAdmin;
        UserType = UserTypeConstants.User;

        SecurityStamp = Guid.NewGuid().ToString();
        Status = UserStatus.PendingActivation;

        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserCreatedEvent(Id, Email));
    }

    // Factory method
    public static User Create(
        string email,
        string passwordHash,
        string? firstName,
        string? lastName,
        Guid tenantId,
        IEnumerable<Guid> defaultRoleIds,
        bool isTenantAdmin = false
    )
    {
        var user = new User(email, passwordHash, firstName, lastName, false);

        // Tạo quan hệ Tenant
        var userTenant = new UserTenant(user.Id, tenantId, isTenantAdmin);

        user.RaiseDomainEvent(new UserCreatedEvent(user.Id, user.Email));

        // Gán role mặc định
        foreach (var roleId in defaultRoleIds)
        {
            userTenant.AddRole(roleId);
        }

        user.UserTenants.Add(userTenant);

        return user;
    }
    public static User CreateSystemAdmin(
        string email,
        string passwordHash)
    {
        var user = new User()
        {
            Email = email,
            PasswordHash = passwordHash,
            IsSystemAdmin = true,
            Status = UserStatus.Active,
            UserType = UserTypeConstants.SystemAdmin,
            CreatedAt = DateTime.UtcNow
        };

        user.RaiseDomainEvent(new SystemAdminCreatedEvent(user.Id, user.Email));

        return user;
    }
    public void PromoteToSystemAdmin()
    {
        IsSystemAdmin = true;
        UserType = UserTypeConstants.SystemAdmin;
    }
    // Methods
    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }

    // =========================
    // LOGIN DOMAIN LOGIC
    // =========================
    public bool IsLocked()
    {
        return LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow;
    }
    public void ConfirmEmail()
    {
        if (EmailConfirmed)
            return;

        EmailConfirmed = true;

        if (Status == UserStatus.PendingActivation)
        {
            Status = UserStatus.Active;
        }

        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }

    public void ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }

    public void UpdateProfile(string? firstName, string? lastName, string? phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new UserProfileUpdatedEvent(Id));
    }
    public void AddToTenant(Guid tenantId, bool isTenantAdmin, IEnumerable<Guid> roleIds)
    {
        var userTenant = new UserTenant(Id, tenantId, isTenantAdmin);

        foreach (var roleId in roleIds)
            userTenant.AddRole(roleId);

        UserTenants.Add(userTenant);
    }
    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        FailedLoginAttempts = 0;
        LockoutEnd = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedLogin(int maxAttempts = 5)
    {
        FailedLoginAttempts++;

        if (FailedLoginAttempts >= maxAttempts)
        {
            LockoutEnd = DateTime.UtcNow.AddMinutes(30);
            Status = UserStatus.Locked;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Unlock()
    {
        FailedLoginAttempts = 0;
        LockoutEnd = null;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
    // =========================
    // SECURITY
    // =========================
    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SecurityStamp = Guid.NewGuid().ToString();
        UpdatedAt = DateTime.UtcNow;
    }
    public void Activate()
    {
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = UserStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Lock()
    {
        Status = UserStatus.Locked;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Delete(Guid? deletedBy = null)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
        Status = UserStatus.Deleted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Restore()
    {
        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }
    public void ChangeLanguage(Guid languageId, Guid? updatedByUserId)
    {
        LanguageId = languageId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByUserId;

        RaiseDomainEvent(new UserLanguageChangedEvent(Id, languageId, updatedByUserId));
    }
}