using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Infrastructures.Persistence.Configurations.StudentManagement;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.StudentCode)
            .IsUnique();

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.IdentityCardNumber)
            .HasMaxLength(50);

        builder.HasIndex(x => x.IdentityCardNumber)
            .IsUnique()
            .HasFilter("[IdentityCardNumber] IS NOT NULL");

        // Đảm bảo quan hệ 1 - 1 (1 User chỉ gắn duy nhất 1 Student)
        builder.HasIndex(s => s.UserId)
            .IsUnique()
            .HasFilter("[UserId] IS NOT NULL");
    }
}