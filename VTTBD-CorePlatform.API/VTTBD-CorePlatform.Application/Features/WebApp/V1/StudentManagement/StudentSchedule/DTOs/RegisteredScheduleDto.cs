using System;
using System.Collections.Generic;

namespace VTTBD_CorePlatform.Application.Features.WebApp.V1.StudentManagement.StudentSchedule.DTOs;

// Class DTO chứa thông tin tổng quan (phần trên của giao diện)
public class RegisteredScheduleSummaryDto
{
    public int TotalCourses { get; set; }           // Số học phần đã đăng ký (vd: 7)
    public int TotalCredits { get; set; }           // Tổng số tín chỉ đã đăng ký (vd: 17)
    public decimal TotalAmount { get; set; }        // Tổng số tiền dự kiến nộp (vd: 12,580,000)
    public List<RegisteredCourseItemDto> Courses { get; set; } = new(); // Danh sách các môn học
}

// Class DTO chứa chi tiết từng dòng học phần trong bảng
public class RegisteredCourseItemDto
{
    public Guid Id { get; set; }
    public string CourseType { get; set; } = default!;     // Tính chất môn học
    public string CourseCode { get; set; } = default!;     // Mã học phần
    public string CourseName { get; set; } = default!;     // Tên học phần
    public int Credits { get; set; }                       // Số tín chỉ
    public string ClassName { get; set; } = default!;      // Tên lớp tín chỉ
    public string Shift { get; set; } = default!;          // Ca học
    public string Schedule { get; set; } = default!;       // Lịch học
    public string LecturerName { get; set; } = default!;   // Giảng viên
    public string RoomName { get; set; } = default!;       // Phòng học
    public decimal UnitPrice { get; set; }                 // Đơn giá
    public decimal AmountPaid { get; set; }                // Số tiền nộp
    public bool IsApproved { get; set; }                   // Cố vấn duyệt
}