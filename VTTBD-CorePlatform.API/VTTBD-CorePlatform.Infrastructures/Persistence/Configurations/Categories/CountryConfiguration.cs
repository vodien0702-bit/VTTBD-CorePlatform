using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using VTTBD_CorePlatform.Domain.Entities.System.Categories;

namespace VTTBD_CorePlatform.Infrastructures.Persistence.Configurations.Categories;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        // Tạo Index Unique để tránh trùng lặp tên quốc gia
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}