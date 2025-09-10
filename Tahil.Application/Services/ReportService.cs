namespace Tahil.Application.Services;

public class ReportService(IEnumerable<IReport> reports) : IReportService
{
    public async Task<byte[]> GenerateAsync<T>(ReportType type, T dataSet) where T : BaseDto
    {
        var report = reports.FirstOrDefault(r => r.Type == type);
        if (report == null)
        {
            throw new InvalidOperationException($"No report found for type: {type}");
        }

        return await report.GenerateAsync(dataSet);
    }
}