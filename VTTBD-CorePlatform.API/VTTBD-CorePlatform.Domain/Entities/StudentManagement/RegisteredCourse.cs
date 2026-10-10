using System;
using VTTBD_CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.Domain.Entities.StudentManagement;

public class RegisteredCourse : AggregateRoot<Guid>
{
    public Guid StudentId { get; private set; }
    public Guid SemesterId { get; private set; }

    public string CourseType { get; private set; } = default!;      // Tính chất môn học (vd: Học bình thường ngành 1)
    public string CourseCode { get; private set; } = default!;      // Mã học phần (vd: CNTT203V1)
    public string CourseName { get; private set; } = default!;      // Tên học phần
    public int Credits { get; private set; }                        // Số tín chỉ
    public string ClassName { get; private set; } = default!;       // Tên lớp tín chỉ (vd: CNTT203V1.1_LT)
    public string Shift { get; private set; } = default!;           // Ca học (Sáng/Tối)
    public string Schedule { get; private set; } = default!;        // Lịch học (vd: Thứ 2(T4-6))
    public string LecturerName { get; private set; } = default!;    // Giảng viên
    public string RoomName { get; private set; } = default!;        // Phòng học
    public decimal UnitPrice { get; private set; }                  // Đơn giá
    public decimal AmountPaid { get; private set; }                 // Số tiền nộp
    public bool IsApproved { get; private set; }                    // Cố vấn duyệt

    public Semester Semester { get; private set; } = default!;

    private RegisteredCourse() { }

    public RegisteredCourse(
        Guid studentId, Guid semesterId, string courseType, string courseCode,
        string courseName, int credits, string className, string shift,
        string schedule, string lecturerName, string roomName, decimal unitPrice, bool isApproved)
    {
        Id = Guid.NewGuid();
        StudentId = studentId;
        SemesterId = semesterId;
        CourseType = courseType;
        CourseCode = courseCode;
        CourseName = courseName;
        Credits = credits;
        ClassName = className;
        Shift = shift;
        Schedule = schedule;
        LecturerName = lecturerName;
        RoomName = roomName;
        UnitPrice = unitPrice;
        AmountPaid = credits * unitPrice;
        IsApproved = isApproved;
        CreatedAt = DateTime.UtcNow;
    }
}