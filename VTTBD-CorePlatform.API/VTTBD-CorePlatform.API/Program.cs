using Microsoft.EntityFrameworkCore;
using VTTBD_CorePlatform.Application.Abstractions.Authentication;
using VTTBD_CorePlatform.Application.Abstractions.Localization;
using VTTBD_CorePlatform.Application.Abstractions.MultiTenants;
using VTTBD_CorePlatform.Application.Abstractions.Persistence;
using VTTBD_CorePlatform.Application.Common.Models;
using VTTBD_CorePlatform.Infrastructures.Persistence;
using VTTBD_CorePlatform.Infrastructures.Persistence.DbContext;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Đăng ký DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Đăng ký IUnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 4. Đăng ký các dịch vụ phụ trợ (Giải quyết triệt để lỗi DI của MediatR)
builder.Services.AddScoped<ISystemMessageProvider, DummySystemMessageProvider>();
builder.Services.AddScoped<ICurrentUser, DummyCurrentUser>();
builder.Services.AddScoped<ICurrentTenant, DummyCurrentTenant>();

// 5. Đăng ký MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Result).Assembly));

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// --- Bổ sung các Dummy Class bên dưới Program.cs để thỏa mãn DI ---
public class DummySystemMessageProvider : ISystemMessageProvider
{
    public System.Threading.Tasks.Task<string> GetAsync(string code) => System.Threading.Tasks.Task.FromResult(code);
}

public class DummyCurrentUser : ICurrentUser
{
    public Guid UserId => Guid.Parse("ECC57A70-3B3C-43A8-AE9D-910398504F1C");
    public string? UserName => "Võ Hoàng Diện";
}

public class DummyCurrentTenant : ICurrentTenant
{
    public Guid? TenantId => null;
}