using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection.Emit;
using VTTBD_CorePlatform.Domain.Common.Events;
using VTTBD_CorePlatform.Domain.Entities.Identities;
using VTTBD_CorePlatform.Domain.Entities.StudentManagement;

namespace VTTBD_CorePlatform.Infrastructures.Persistence.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nạp toàn bộ Configuration từ assembly này
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Bỏ qua DomainEvent không sinh vào DB
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.Ignore<EntitySoftDeletedEvent<Guid>>();
        modelBuilder.Ignore<EntityRestoredEvent<Guid>>();

        // Tự động thêm Global Filter cho Soft Delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            ApplySoftDeleteFilter(modelBuilder, entityType);
        }
    }

    private void ApplySoftDeleteFilter(ModelBuilder modelBuilder, IMutableEntityType entityType)
    {
        var prop = entityType.FindProperty("IsDeleted");
        if (prop == null) return;

        var parameter = Expression.Parameter(entityType.ClrType, "e");
        var body = Expression.Equal(
            Expression.Property(parameter, "IsDeleted"),
            Expression.Constant(false)
        );

        var lambda = Expression.Lambda(body, parameter);
        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
    }
}