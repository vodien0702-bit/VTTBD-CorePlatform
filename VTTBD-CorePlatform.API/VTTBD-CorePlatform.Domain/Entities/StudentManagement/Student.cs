using System;
using VTTBD_CorePlatform.Domain.Common.Entities;
using VTTBD_CorePlatform.Domain.Entities.Identities;

namespace VTTBD_CorePlatform.Domain.Entities.StudentManagement;

public class Student : AggregateRoot<Guid>
{
    // Thông tin cơ bản
    public string StudentCode { get; private set; } = default!;
    public string FullName { get; private set; } = default!;
    public DateOnly? DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public string? PlaceOfBirth { get; private set; }
    public string? Hometown { get; private set; }
    public string? Nationality { get; private set; }
    public string? Ethnicity { get; private set; }
    public string? Religion { get; private set; }
    public string? SocialClass { get; private set; }
    public DateOnly? YouthUnionJoinDate { get; private set; }
    public DateOnly? CommunistPartyJoinDate { get; private set; }

    // Thường trú & Liên lạc
    public string? PermanentAddress { get; private set; }
    public string? PermanentWard { get; private set; }
    public string? PermanentDistrict { get; private set; }
    public string? PermanentProvince { get; private set; }
    public string? CurrentAddress { get; private set; }
    public string? MailingAddress { get; private set; }

    // Chính sách
    public string? PolicyTarget { get; private set; }
    public string? AllowanceTarget { get; private set; }
    public string? TargetGroup { get; private set; }

    // Giấy tờ
    public string? HomePhoneNumber { get; private set; }
    public string? PersonalPhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string? IdentityCardNumber { get; private set; }

    // Khóa ngoại trỏ đến User (Quan hệ 1 - 1)
    public Guid? UserId { get; private set; }
    public User? User { get; private set; }

    private Student() { }

    public Student(string studentCode, string fullName, Guid? userId = null)
    {
        Id = Guid.NewGuid();
        StudentCode = studentCode;
        FullName = fullName;
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void LinkToUser(Guid userId)
    {
        UserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
}