namespace LargeExcelOperation.Service.Interfaces;

public interface IExcelService
{
    Task<byte[]> CreateExcelWithNpoiAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelWithLargeXlsxAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelWithEpPlusAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelWithNpoiAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelWithLargeXlsxAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data);
    Task<byte[]> CreateExcelEpPlusAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data);
}