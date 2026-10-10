using System;
using VTTBD_CorePlatform.Domain.Common.Entities;

namespace VTTBD_CorePlatform.Domain.Entities.StudentManagement;

public class Semester : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!; // Ví dụ: Đợt 1 Học kỳ 1 Năm học 2026-2027
    public string AcademicYear { get; private set; } = default!; // 2026-2027
    public int Term { get; private set; } // Học kỳ 1, 2...
    public int Batch { get; private set; } // Đợt 1, 2...

    private Semester() { }

    public Semester(string name, string academicYear, int term, int batch)
    {
        Id = Guid.NewGuid();
        Name = name.Trim();
        AcademicYear = academicYear;
        Term = term;
        Batch = batch;
        CreatedAt = DateTime.UtcNow;
    }
}