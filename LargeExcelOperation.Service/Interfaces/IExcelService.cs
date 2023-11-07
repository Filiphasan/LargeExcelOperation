using LargeExcelOperation.Core.Models.Report;

namespace LargeExcelOperation.Service.Interfaces;

public interface IExcelService
{
    Task<byte[]> CreateExcelWithNpoiAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelWithLargeXlsxAsync<TEntity>(IEnumerable<TEntity> data);
}