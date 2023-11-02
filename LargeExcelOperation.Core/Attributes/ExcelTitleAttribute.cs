namespace LargeExcelOperation.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ExcelTitleAttribute : Attribute
{
    public string Title { get; }

    public ExcelTitleAttribute(string title)
    {
        Title = title;
    }
}