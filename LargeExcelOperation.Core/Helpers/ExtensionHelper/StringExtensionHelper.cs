namespace LargeExcelOperation.Core.Helpers.ExtensionHelper;

public static class StringExtensionHelper
{
    public static string ToCustomCurrency(this decimal value)
    {
        return value.ToString("N");
    }
}