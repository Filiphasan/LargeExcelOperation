using System.Reflection;
using LargeExcelOperation.Core.Attributes;
using LargeExcelOperation.Core.Models;

namespace LargeExcelOperation.Service.Helpers;

public static class PropertyHelper
{
    public static PropertyInfo[] GetModelProperties<TModel>()
    {
        var type = typeof(TModel);
        var properties = type.GetProperties();
        return properties;
    }

    public static ExcelTitleModel[] GetExcelTitleModels<TModel>(IEnumerable<PropertyInfo>? properties = null)
    {
        properties ??= GetModelProperties<TModel>();
        var titles = properties.Select(propertyInfo =>
        {
            var titleModel = new ExcelTitleModel()
            {
                Title = propertyInfo.Name,
                PropertyName = propertyInfo.Name,
            };
            if (Attribute.GetCustomAttribute(propertyInfo, typeof(ExcelTitleAttribute)) is ExcelTitleAttribute attribute)
            {
                titleModel.Title = attribute.Title;
            }

            return titleModel;
        }).ToArray();

        return titles;
    }
}