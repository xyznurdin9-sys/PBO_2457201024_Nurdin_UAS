using Microsoft.EntityFrameworkCore;
using SoundRentalApp.Data;
using SoundRentalApp.Services;
using SoundRentalApp.Services.Interfaces;
using SoundRentalApp.Services.Pdf;
using SoundRentalApp.Services.Excel;



var builder = WebApplication.CreateBuilder(args);

// ========================================
// QuestPDF License
// ========================================
QuestPDF.Settings.License =
    QuestPDF.Infrastructure.LicenseType.Community;

// ========================================
// MVC
// ========================================
builder.Services.AddControllersWithViews();

// ========================================
// Database
// ========================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// ========================================
// Dependency Injection
// ========================================
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IExcelService, ExcelService>();

// ========================================

var app = builder.Build();

// ========================================
// Configure HTTP Pipeline
// ========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();