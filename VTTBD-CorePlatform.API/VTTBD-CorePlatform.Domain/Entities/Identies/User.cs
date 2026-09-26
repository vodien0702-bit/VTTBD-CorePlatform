using System;
using VTTBD_CorePlatform.Domain.Common.Entities;
using VTTBD_CorePlatform.Domain.Common.Enums;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Domain.Entities.Identities;

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
    public bool IsSystemAdmin { get; private set; } = false;

    // Navigation Property quan hệ 1 - 1 với Student
    public Student? Student { get; private set; }

    // Constructor cho EF Core
    private User() { }

    public User(string userName, string email, string passwordHash, string? firstName = null, string? lastName = null)
    {
        Id = Guid.NewGuid();
        UserName = userName.Trim();
        Email = email.Trim().ToLower();
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        SecurityStamp = Guid.NewGuid().ToString();
        Status = UserStatus.PendingActivation;
        CreatedAt = DateTime.UtcNow;
    }

    public string GetFullName() => $"{FirstName} {LastName}".Trim();

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        Status = UserStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SecurityStamp = Guid.NewGuid().ToString();
        UpdatedAt = DateTime.UtcNow;
    }
}