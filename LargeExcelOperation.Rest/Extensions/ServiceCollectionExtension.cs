using LargeExcelOperation.Service.Implementations;
using LargeExcelOperation.Service.Interfaces;

namespace LargeExcelOperation.Rest.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterService(this IServiceCollection services)
    {
        services.AddSingleton<IExcelService, ExcelService>();
        services.AddScoped<IReportService, ReportService>();
    }
}