using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.CorePlatform.Domain.Common.Entities;
using VTTBD_CorePlatformTBDUni.CorePlatform.Domain.Entities.Identity;

namespace VTTBD_CorePlatform.Domain.Entities.StudentManagement;

public class Student : AggregateRoot<Guid>
{
    public Guid StudentId { get; private set; } = default!;

    // Thông tin cơ bản
    public string StudentCode { get; private set; } = default!; // Mã sinh viên
    public string FullName { get; private set; } = default!;    // Họ và tên
    public DateOnly? DateOfBirth { get; private set; }          // Ngày sinh
    public string? Gender { get; private set; }                 // Giới tính
    public string? PlaceOfBirth { get; private set; }           // Nơi sinh
    public string? Hometown { get; private set; }               // Quê quán
    public string? Nationality { get; private set; }            // Quốc tịch
    public string? Ethnicity { get; private set; }              // Dân tộc
    public string? Religion { get; private set; }               // Tôn giáo
    public string? SocialClass { get; private set; }            // TP xuất thân
    public DateOnly? YouthUnionJoinDate { get; private set; }   // Ngày vào Đoàn
    public DateOnly? CommunistPartyJoinDate { get; private set; }// Ngày vào Đảng

    public Guid? UserId { get; private set; }


    // Nơi thường trú
    public string? PermanentAddress { get; private set; }       // Nơi thường trú
    public string? PermanentWard { get; private set; }          // Xã/phường
    public string? PermanentDistrict { get; private set; }      // Quận/huyện
    public string? PermanentProvince { get; private set; }      // Tỉnh/TP

    // Đối tượng chính sách & Trợ cấp
    public string? PolicyTarget { get; private set; }           // Đối tượng CS
    public string? AllowanceTarget { get; private set; }        // Đối tượng trợ cấp
    public string? TargetGroup { get; private set; }            // Nhóm ĐT

    // Liên lạc & Giấy tờ
    public string? HomePhoneNumber { get; private set; }        // ĐT nhà riêng
    public string? PersonalPhoneNumber { get; private set; }    // ĐT cá nhân
    public string? Email { get; private set; }                  // Email
    public string? IdentityCardNumber { get; private set; }     // Số CMND / CCCD
    public string? MailingAddress { get; private set; }         // Địa chỉ báo tin
    public string? CurrentAddress { get; private set; }         // Nơi ở hiện nay

    // Navigation Properties
    public User? User { get; private set; }
}
