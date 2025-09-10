namespace Tahil.Domain.Services;

public interface IReportService
{
    Task<byte[]> GenerateAsync<T>(ReportType type, T dataSet) where T : BaseDto;
}

public interface IReport 
{
    ReportType Type { get; }
    Task<byte[]> GenerateAsync<T>(T dataSet) where T : BaseDto;
}