using Microsoft.AspNetCore.Mvc;
using SoundRentalApp.Services.Interfaces;


namespace SoundRentalApp.Controllers;

public class ReportController : Controller
{
    private readonly IReportService _reportService;
    private readonly IPdfService _pdfService;
    private readonly IExcelService _excelService;

    public ReportController(
        IReportService reportService,
        IPdfService pdfService,
        IExcelService excelService)
    {
        _reportService = reportService;
        _pdfService = pdfService;
        _excelService = excelService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Daily(DateTime? date)
    {
        var reportDate = date ?? DateTime.Today;

        ViewBag.Date = reportDate;

        return View(await _reportService.GetDailyReportAsync(reportDate));
    }

    public async Task<IActionResult> Monthly(int? month, int? year)
    {
        int m = month ?? DateTime.Today.Month;
        int y = year ?? DateTime.Today.Year;

        ViewBag.Month = m;
        ViewBag.Year = y;

        return View(await _reportService.GetMonthlyReportAsync(m, y));
    }

    public async Task<IActionResult> Yearly(int? year)
    {
        int y = year ?? DateTime.Today.Year;

        ViewBag.Year = y;

        return View(await _reportService.GetYearlyReportAsync(y));
    }

    public async Task<IActionResult> Customer()
    {
        return View(await _reportService.GetCustomerReportAsync());
    }

    public async Task<IActionResult> Equipment()
    {
        return View(await _reportService.GetEquipmentReportAsync());
    }

    // ===========================
    // EXPORT DAILY PDF
    // ===========================
    public async Task<IActionResult> DailyPdf(DateTime? date)
    {
        var reportDate = date ?? DateTime.Today;

        var rentals = await _reportService
            .GetDailyReportAsync(reportDate);

        var pdf = _pdfService.GenerateDailyReport(
            rentals,
            reportDate);

        return File(
            pdf,
            "application/pdf",
            $"DailyReport_{reportDate:yyyyMMdd}.pdf");
    }
    public async Task<IActionResult> DailyExcel(DateTime? date)
    {
        var reportDate = date ?? DateTime.Today;

        var rentals = await _reportService.GetDailyReportAsync(reportDate);

        var excel = _excelService.DailyReport(rentals, reportDate);

        return File(
            excel,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"DailyReport_{reportDate:yyyyMMdd}.xlsx");
    }

}